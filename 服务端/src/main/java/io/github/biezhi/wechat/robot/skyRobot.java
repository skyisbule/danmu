package io.github.biezhi.wechat.robot;

import com.google.gson.JsonObject;
import io.github.biezhi.wechat.Server;
import io.github.biezhi.wechat.handle.AbstractMessageHandler;
import io.github.biezhi.wechat.model.GroupMessage;
import io.github.biezhi.wechat.model.UserMessage;

import java.net.SocketException;
import java.net.UnknownHostException;


public class skyRobot extends AbstractMessageHandler {

    @Override
    public void userMessage(UserMessage userMessage) throws SocketException, UnknownHostException {
        if (null == userMessage) {
            return;
        }

        JsonObject raw_msg = userMessage.getRawMsg();
        String toUid = raw_msg.get("FromUserName").getAsString();
        System.out.println(raw_msg);
        //userMessage.sendText("这条信息来自机器人，请无视它。",toUid);


        String text = userMessage.getText();
        if (text==null)
            return;
        Server.sendData(text);
    }

    @Override
    public void groupMessage(GroupMessage groupMessage) throws SocketException, UnknownHostException {
        String text = groupMessage.getText();
        if (text==null)
            return;
        Server.sendData(text);
    }
}
