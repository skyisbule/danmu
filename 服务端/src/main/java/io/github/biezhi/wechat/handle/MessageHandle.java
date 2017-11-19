package io.github.biezhi.wechat.handle;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import io.github.biezhi.wechat.model.GroupMessage;
import io.github.biezhi.wechat.model.UserMessage;

import java.net.SocketException;
import java.net.UnknownHostException;

/**
 * 一个默认的消息处理实现
 *
 * @author biezhi
 * 17/06/2017
 */
public interface MessageHandle {

    /**
     * 保存微信消息
     *
     * @param msg
     */
    void wxSync(JsonObject msg);

    void userMessage(UserMessage userMessage) throws SocketException, UnknownHostException;

    void groupMessage(GroupMessage groupMessage) throws SocketException, UnknownHostException;

    void groupMemberChange(String groupId, JsonArray memberList);

    void groupListChange(String groupId, JsonArray memberList);

}
