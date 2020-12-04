package main

import (
	"fmt"
	"github.com/MissGod1/PProxy/common"
	"github.com/MissGod1/PProxy/proxy/shadowsocks"
	"github.com/eycorsican/go-tun2socks/common/log"
	"github.com/eycorsican/go-tun2socks/core"
	"strconv"
	"time"
)

func init()  {
	RegisterHandler("shadowsocks", func() {
		//_, err := net.ResolveIPAddr("tcp", server.Server)
		//if err != nil {
		//	log.Fatalf("invalid proxy server address: %v", err)
		//}
		port, err := strconv.Atoi(server.ServerPort)
		if server.Plugin != "" {
			plugin = common.NewPlugin()

			if err != nil {
				panic("Port Error.")
			}
			localAddr, err := plugin.StartPlugin(server.Plugin, server.PluginOpts, fmt.Sprintf("%v:%v", server.Server, port), false)
			if err != nil {
				log.Fatalf("start plugin failed.")
			}
			core.RegisterTCPConnHandler(shadowsocks.NewTCPHandler(localAddr, server.Method, server.Password, fakeDns))
		}else {
			core.RegisterTCPConnHandler(shadowsocks.NewTCPHandler(core.ParseTCPAddr(server.Server, uint16(port)).String(), server.Method, server.Password, fakeDns))
		}

		core.RegisterUDPConnHandler(shadowsocks.NewUDPHandler(core.ParseUDPAddr(server.Server, uint16(port)).String(), server.Method, server.Password, 1*time.Second, fakeDns))
	})
}