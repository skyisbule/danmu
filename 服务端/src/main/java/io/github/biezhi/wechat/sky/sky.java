package io.github.biezhi.wechat.sky;

import com.google.gson.JsonObject;
import io.github.biezhi.wechat.Utils;
import io.github.biezhi.wechat.handle.AbstractMessageHandler;
import io.github.biezhi.wechat.model.GroupMessage;
import io.github.biezhi.wechat.model.UserMessage;

/**
 * Created by skyisbule on 2017/11/10.
 * 自己实现的机器人
 */
public class sky extends AbstractMessageHandler {


    @Override
    public void userMessage(UserMessage userMessage) {
        if (null == userMessage) {
            return;
        }
        //发来的消息
        String text = userMessage.getText();
        JsonObject raw_msg = userMessage.getRawMsg();
        //获取发送人id
        String toUid = raw_msg.get("FromUserName").getAsString();
        //发送结果
        String result = null;
        userMessage.sendText(result, toUid);
    }

    @Override
    public void groupMessage(GroupMessage groupMessage) {
        System.out.println(groupMessage);
        String text = groupMessage.getText();
        if (Utils.isNotBlank(text)) {
            String result = null ;
            groupMessage.sendText(result, groupMessage.getGroupId());
        }
    }

}
