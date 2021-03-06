package main

import (
	"encoding/base64"
	"encoding/json"
	"flag"
	"github.com/MissGod1/PProxy/common/dns"
	"github.com/MissGod1/PProxy/common/dns/fakedns"
	"github.com/eycorsican/go-tun2socks/common/log"
	_ "github.com/eycorsican/go-tun2socks/common/log/simple"
	"github.com/eycorsican/go-tun2socks/core"
	"io/ioutil"
	"os"
	"os/signal"
	"strings"
	"syscall"

	"github.com/MissGod1/PProxy/common"
)

// 服务配置
type Server struct {
	Type       string `json:"type"`
	Server     string `json:"server"`
	ServerPort string `json:"server_port"`
	Password   string `json:"password"`
	Method     string `json:"method"`

	Plugin     string `json:"plugin"`
	PluginOpts string `json:"plugin_opts"`
}

// 进程配置
type Process struct {
	Processes []string `json:"processes"`
	Whitelist []string `json:"whitelist"`
}

var server *Server
var process *Process
var plugin *common.Plugin
var fakeDns dns.FakeDns

var createrhandler = make(map[string]func())

func RegisterHandler(key string, creater func()) {
	createrhandler[key] = creater
}

func GetServer(file string) *Server {
	data, err := ioutil.ReadFile(file)
	if err != nil {
		panic("Open Server Configure File Error!")
	}
	s := Server{}
	err = json.Unmarshal(data, &s)
	if err != nil {
		panic("Server Configure File Have some issue!")
	}
	return &s
}

func GetProcess(file string) *Process {
	data, err := ioutil.ReadFile(file)
	if err != nil {
		panic("Open Server Configure File Error!")
	}

	p := Process{}
	err = json.Unmarshal(data, &p)
	if err != nil {
		panic("Server Configure File Have some issue!")
	}
	return &p
}

func main() {
	sconfig := flag.String("sconfig", "", "server configure file")
	pconfig := flag.String("pconfig", "", "process configure file")
	ss := flag.String("s", "", "string args b64")
	logLevel := flag.String("log", "info", "log level")

	flag.Parse()
	if *ss != "" {
		server, process = GetFromString(*ss)
	} else if *sconfig != "" && *pconfig != "" {
		server = GetServer(*sconfig)
		process = GetProcess(*pconfig)
	} else {
		flag.Usage()
		os.Exit(1)
	}
	switch strings.ToLower(*logLevel) {
	case "info":
		log.SetLevel(log.INFO)
	case "debug":
		log.SetLevel(log.DEBUG)
	case "warn":
		log.SetLevel(log.WARN)
	case "none":
		log.SetLevel(log.NONE)
	case "error":
		log.SetLevel(log.ERROR)
	default:
		log.SetLevel(log.INFO)
	}

	fakeDns = fakedns.NewSimpleFakeDns()
	if creater, found := createrhandler[server.Type]; found {
		creater()
	} else {
		panic("Unsupported proxy type.")
	}
	app, err := NewApp(server, process)
	if err != nil {
		panic("App Run Failed.")
	}
	core.RegisterOutputFn(app.Write)
	lwip := core.NewLWIPStack()
	_, _ = app.WriteTo(lwip)

	sigCh := make(chan os.Signal, 1)
	signal.Notify(sigCh, syscall.SIGINT, syscall.SIGTERM)
	<-sigCh
	if plugin != nil {
		plugin.KillPlugin()
	}
	app.Close()
}

func GetFromString(s string) (*Server, *Process) {
	ss := strings.Split(s, ":")
	process := &Process{}
	server := &Server{}
	raw, err := base64.StdEncoding.DecodeString(ss[0])
	if err != nil {
		panic("Decode args Failed.")
	}
	err = json.Unmarshal(raw, process)
	if err != nil {
		panic("Parse process Failed.")
	}
	raw, err = base64.StdEncoding.DecodeString(ss[1])
	if err != nil {
		panic("Decode args Failed.")
	}
	err = json.Unmarshal(raw, server)
	if err != nil {
		panic("Parse server Failed.")
	}
	return server, process
}
