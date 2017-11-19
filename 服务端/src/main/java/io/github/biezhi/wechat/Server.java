package io.github.biezhi.wechat;

import java.net.*;
import java.io.*;

public class Server {

    public static void sendData(String str)throws SocketException, UnknownHostException{
        DatagramSocket ds = new DatagramSocket();// 创建用来发送数据报包的套接字
        DatagramPacket dp = new DatagramPacket(str.getBytes(),str.getBytes().length,
                                    InetAddress.getByName("127.0.0.1"), 9999);
        // 构造数据报包，用来将长度为 length 的包发送到指定主机上的指定端口号
        try {
            ds.send(dp);
        } catch (IOException e) {
            e.printStackTrace();
        }
        ds.close();

    }

}
