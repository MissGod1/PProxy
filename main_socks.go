package main

import (
	"fmt"
	"github.com/MissGod1/PProxy/proxy/socks"
	"github.com/eycorsican/go-tun2socks/common/log"
	"github.com/eycorsican/go-tun2socks/core"
	"net"
	"strconv"
	"time"
)

func init()  {
	RegisterHandler("socks5", func() {
		// Verify proxy server address.
		port, err := strconv.Atoi(server.ServerPort)
		if err != nil {
			panic("Port Error.")
		}
		_, err = net.ResolveTCPAddr("tcp",fmt.Sprintf("%v:%v", server.Server, port))
		if err != nil {
			log.Fatalf("invalid proxy server address: %v", err)
		}
		//proxyHost := proxyAddr.IP.String()
		//proxyPort := uint16(proxyAddr.Port)

		core.RegisterTCPConnHandler(socks.NewTCPHandler(server.Server, uint16(port), fakeDns))
		core.RegisterUDPConnHandler(socks.NewUDPHandler(server.Server, uint16(port), 1*time.Second, fakeDns))
	})
}
