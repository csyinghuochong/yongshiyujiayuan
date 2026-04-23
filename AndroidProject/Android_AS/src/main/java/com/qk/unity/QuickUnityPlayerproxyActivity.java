package com.qk.unity;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.os.Handler.Callback;
import android.os.Message;
import android.provider.Settings;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.text.TextUtils;
import android.util.Log;
import android.widget.Toast;

import com.quicksdk.BaseCallBack;
import com.quicksdk.Extend;
import com.quicksdk.FuncType;
import com.quicksdk.Payment;
import com.quicksdk.QuickSDK;
import com.quicksdk.Sdk;
import com.quicksdk.User;
import com.quicksdk.entity.GameRoleInfo;
import com.quicksdk.entity.OrderInfo;
import com.quicksdk.entity.ShareInfo;
import com.quicksdk.entity.UserInfo;
import com.quicksdk.notifier.ExitNotifier;
import com.quicksdk.notifier.InitNotifier;
import com.quicksdk.notifier.LoginNotifier;
import com.quicksdk.notifier.LogoutNotifier;
import com.quicksdk.notifier.PayNotifier;
import com.quicksdk.notifier.SwitchAccountNotifier;
import com.quicksdk.utility.AppConfig;
import com.unity3d.player.UnityPlayer;

@SuppressLint("NewApi")
public abstract class QuickUnityPlayerproxyActivity extends com.unity3d.player.UnityPlayerActivity implements Callback {

    private static final String TAG = "quicksdk_unity_support";

    private static final int MSG_LOGIN = 101;
    private static final int MSG_LOGOUT = 102;
    private static final int MSG_PAY = 103;
    private static final int MSG_EXIT = 104;
    private static final int MSG_ROLEINFO = 105;
    private static final int MSG_EXTEND_FUNC = 106;
    private static final int MSG_EXTEND_CALLPLUGIN = 107;
    private static final int MSG_EXTEND_FUNC_SHARE = 108;
    private static final int MSG_EXTEND_FUNC_GOODSINFO = 111;
    private static final int MSG_EXTEND_FUNC_GET_DEVICE_ID = 112;
    private static final int MSG_INIT = 120;
    private static final int MSG_EXTEND_FUNC_WiTH_PARAMS = 901;

    private final int REQUEST_RECORD_PERMISSION_SETTING = 999;

    private static final int INIT_SUCCESS = 1;
    private static final int INIT_FAILED = -1;
    private static final int INIT_DEFAULT = 0;

    private QuickUnityInitNotify initNotify = new QuickUnityInitNotify();
    private QuickUnityLoginNotify loginNotify = new QuickUnityLoginNotify();
    private QuickUnitySwitchAccountNotify switchAccountNotify = new QuickUnitySwitchAccountNotify();
    private QuickUnityLogoutNotify logoutNotify = new QuickUnityLogoutNotify();
    private QuickUnityPayNotify payNotify = new QuickUnityPayNotify();
    private QuickUnityExitNotiry exitNotiry = new QuickUnityExitNotiry();

    private String gameObjectName;

    private int initState = INIT_DEFAULT;

    private String mInitMsg = "";

    Handler mHandler = new Handler(this);

    boolean isLancScape = true;

    private Activity mActivity = null;
    private static String goodsInfos = null;
    //
    private boolean isInited = false;

    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
        mActivity = this;
        Sdk.getInstance().onCreate(this);
        // 探娱初始化
        // doInit();
        // requestInit();
    }

    @Override
    protected void onRestart() {
        Sdk.getInstance().onRestart(this);
        super.onRestart();

    }

    @Override
    protected void onStart() {
        Sdk.getInstance().onStart(this);
        super.onStart();

    }

    @Override
    protected void onResume() {
        Sdk.getInstance().onResume(this);
        super.onResume();

    }

    @Override
    protected void onNewIntent(Intent intent) {
        Sdk.getInstance().onNewIntent(intent);
        super.onNewIntent(intent);

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {

        super.onActivityResult(requestCode, resultCode, data);
        Sdk.getInstance().onActivityResult(this, requestCode, resultCode, data);

    }

    @Override
    protected void onPause() {
        Sdk.getInstance().onPause(this);
        super.onPause();

    }

    @Override
    protected void onStop() {
        Sdk.getInstance().onStop(this);
        super.onStop();

    }

    @Override
    protected void onDestroy() {

        Sdk.getInstance().onDestroy(this);
        super.onDestroy();

    }

    public void doInit() {
        isLancScape = QuickUnityPlayerproxyActivity.this.getResources()
                .getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE;
        QuickSDK.getInstance().setInitNotifier(initNotify).setLoginNotifier(loginNotify).setLogoutNotifier(logoutNotify)
                .setPayNotifier(payNotify).setExitNotifier(exitNotiry).setIsLandScape(isLancScape)
                .setSwitchAccountNotifier(switchAccountNotify);
        Sdk.getInstance().init(this, getProductCode(), getProductKey());

    }


    // ------------------------------------------------------------------------------------
    // -------------------------------------------------------------------------------unity
    // interface
    // ------------------------------------------------------------------------------------

    public void requestInit() {
        mHandler.sendEmptyMessage(MSG_INIT);
    }

    public void requestLogin() {
        mHandler.sendEmptyMessage(MSG_LOGIN);

    }

    public void requestLogout() {
        mHandler.sendEmptyMessage(MSG_LOGOUT);

    }

    public void showPrivace() {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Log.d(TAG, "showPrivace");
                Sdk.getInstance().showPrivace(QuickUnityPlayerproxyActivity.this, new BaseCallBack() {
                    @Override
                    public void onSuccess(Object... objects) {
                        Log.d(TAG, "showPrivace onSuccess: ");
                        callUnityFunc("onPrivaceAgree", "agree");
                    }

                    @Override
                    public void onFailed(Object... objects) {
                        Log.d(TAG, "showPrivace onFailed: ");
                        callUnityFunc("onPrivaceRefuse", "refuse");

                    }
                });
            }
        });

    }

    public void requestPay(String goodsId, String goodsName, String goodsDesc, String quantifier, String cpOrderId,
                           String callbackUrl, String extraParams, String price, String amount, String count, String serverName,
                           String serverId, String roleName, String roleId, String roleBalance, String vipLevel, String roleLevel,
                           String partyName, String roleCreateTime) {

        OrderInfo orderInfo = new OrderInfo();
        orderInfo.setGoodsName(goodsName);
        orderInfo.setGoodsID(goodsId);
        orderInfo.setGoodsDesc(goodsDesc);
        orderInfo.setQuantifier(quantifier);
        orderInfo.setCpOrderID(cpOrderId);
        orderInfo.setCallbackUrl(callbackUrl);
        orderInfo.setExtrasParams(extraParams);
        orderInfo.setPrice(Double.valueOf(price));
        orderInfo.setAmount(Double.valueOf(amount));
        orderInfo.setCount(Integer.valueOf(count));

        GameRoleInfo roleInfo = new GameRoleInfo();
        roleInfo.setServerID(serverId);
        roleInfo.setServerName(serverName);
        roleInfo.setGameRoleID(roleId);
        roleInfo.setGameRoleName(roleName);
        roleInfo.setGameUserLevel(roleLevel);
        roleInfo.setGameBalance(roleBalance);
        roleInfo.setVipLevel(vipLevel);
        roleInfo.setPartyName(partyName);
        roleInfo.setRoleCreateTime(roleCreateTime);

        Message msg = mHandler.obtainMessage(MSG_PAY);
        HashMap<String, Object> mapObj = new HashMap<String, Object>();
        mapObj.put("orderInfo", orderInfo);
        mapObj.put("roleInfo", roleInfo);
        msg.obj = mapObj;
        msg.sendToTarget();

    }

    public void requestExit() {
        mHandler.sendEmptyMessage(MSG_EXIT);
    }


    public void requestExtendFunc(int funcType) {
        Message msg = mHandler.obtainMessage(MSG_EXTEND_FUNC);
//        msg.what = MSG_EXTEND_FUNC;
        msg.arg1 = funcType;
        msg.sendToTarget();
    }

    public void requestCallSDKShare(String title, String content, String imgPath, String imgUrl, String url,
                                    String type, String shareTo, String extenal) {
        ShareInfo shareInfo = new ShareInfo();
        shareInfo.setTitle(title);
        shareInfo.setContent(content);
        shareInfo.setImgPath(imgPath);
        shareInfo.setImgUrl(imgUrl);
        shareInfo.setUrl(url);
        shareInfo.setType(type);
        shareInfo.setShareTo(shareTo);
        shareInfo.setExtenal(extenal);

        Message msg = mHandler.obtainMessage(MSG_EXTEND_FUNC_SHARE);
        msg.obj = shareInfo;
        msg.sendToTarget();

    }



    public void requestCallCustomPlugin(final String roleId, final String roleName, final String serverName,
                                        final String vip) {
        QuickUnityPlayerproxyActivity.this.runOnUiThread(new Runnable() {

            @Override
            public void run() {
                Extend.getInstance().callPlugin(QuickUnityPlayerproxyActivity.this, FuncType.CUSTOM, roleId, roleName,
                        serverName, vip);
            }
        });
    }

    public void requestExtendCallPlugin(final int functype, final String[] data) {

        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Log.d(TAG, "requestExtendCallPlugin: functype == " + functype + "\tdata[0] == " + data[0]);
                Extend.getInstance().callPlugin(QuickUnityPlayerproxyActivity.this, functype, Arrays.stream(data).toArray());
            }
        });

    }

    public String getUserId() {
        if (User.getInstance().getUserInfo(this) != null) {
            Log.e(TAG, "userId->" + User.getInstance().getUserInfo(this).getUID());
            return User.getInstance().getUserInfo(this).getUID();
        } else {
            Log.e(TAG, "user is null");
            return null;
        }
    }

    public String getDeviceID() {
        return Extend.getInstance().getDeviceID(mActivity);
    }

    public void requestUpdateRole(String serverId, String serverName, String roleName, String roleId,
                                  String roleBalance, String vipLevel, String roleLevel, String partyName, String roleCreateTime,
                                  String roleGender, String rolePower, String partId, String professionId, String profession,
                                  String partyRoleId, String partyRoleName, String friendlist, String isCreate) {

        GameRoleInfo roleInfo = new GameRoleInfo();
        roleInfo.setServerID(serverId);
        roleInfo.setServerName(serverName);
        roleInfo.setGameBalance(roleBalance);
        roleInfo.setGameRoleID(roleId);
        roleInfo.setGameRoleName(roleName);
        roleInfo.setVipLevel(vipLevel);
        roleInfo.setGameUserLevel(roleLevel);
        roleInfo.setPartyName(partyName);
        roleInfo.setRoleCreateTime(roleCreateTime);

        roleInfo.setGameRoleGender(roleGender);
        roleInfo.setGameRolePower(rolePower);
        roleInfo.setPartyId(partId);

        roleInfo.setProfessionId(professionId);
        roleInfo.setProfession(profession);
        roleInfo.setPartyId(partyRoleId);
        roleInfo.setPartyRoleName(partyRoleName);
        roleInfo.setFriendlist(friendlist);

        boolean isCreateRole = false;
        if (TextUtils.isEmpty(isCreate) || "false".equalsIgnoreCase(isCreate)) {
            isCreateRole = false;
        } else if ("true".equalsIgnoreCase(isCreate)) {
            isCreateRole = true;
        }

        Message msg = mHandler.obtainMessage(MSG_ROLEINFO);
        if (isCreateRole) {
            msg.arg1 = 1;
        } else {
            msg.arg1 = 0;
        }
        msg.obj = roleInfo;
        msg.sendToTarget();

    }

    public String callFunc(int funcType) {
        Log.d(TAG, "is called:FuncType: " + funcType + ",isFuncSupport: " + isFuncSupport(funcType));
        if (isFuncSupport(funcType)) {
            String resultData = Extend.getInstance().callFunction(this, funcType);
            Log.d(TAG, "callFunc: resultcode == " + resultData);
            return resultData;
        } else {
            return "0";
        }
    }

    public String callFuncWithResult(int funcType) {
        if (isFuncSupport(funcType)) {
            String resultCode = Extend.getInstance().callFunction(this, funcType);
            JSONObject jsonObject = new JSONObject();
            try {
                jsonObject.put("FunctionType", funcType);
                jsonObject.put("msg", resultCode);
                callUnityFunc("onSuccess", jsonObject.toString());
            } catch (JSONException e) {
                Log.e(TAG, e.getMessage());
            }
            return resultCode;
        } else {
            return 0 + "";
        }

    }

    public int callFunc(int funcType, String s) {
        Log.d(TAG, "is called:" + s);
        if (isFuncSupport(funcType)) {
            Log.d(TAG, "is isFuncSupport:" + isFuncSupport(funcType));
            Message msg = mHandler.obtainMessage(MSG_EXTEND_FUNC_GOODSINFO);
            msg.arg1 = funcType;
            msg.obj = s;
            msg.sendToTarget();

            return 1;
        } else {
            return 0;
        }
    }

    public int callFunctionWithParams(int funcType, String[] params) {
        Log.d(TAG, "callFunctionWithParams : functype == " + funcType + "\t params == " + params);
        if (isFuncSupport(funcType)) {
            Log.d(TAG, "is isFuncSupport:" + isFuncSupport(funcType));
            Message msg = mHandler.obtainMessage(MSG_EXTEND_FUNC_WiTH_PARAMS);
            msg.arg1 = funcType;
            msg.obj = params;
            msg.sendToTarget();

            return 1;
        } else {
            return 0;
        }
    }

    public int callTest(Object object) {

        return 1;
    }

    @SuppressLint("LongLogTag")
    public void showMsg(String s) {
        // Toast.makeText(getBaseContext(), s, Toast.LENGTH_SHORT).show();
        Log.d(TAG + " fromUnity", s);
    }

    public boolean isFuncSupport(int funcType) {
        return Extend.getInstance().isFunctionSupported(funcType);
    }

    public void setUnityGameObjectName(String gameObjectName) {
        this.gameObjectName = gameObjectName;
        Log.d(TAG, "gameObjectName=" + gameObjectName);
        switch (initState) {
            case INIT_SUCCESS:
                callUnityFunc("onInitSuccess", new JSONObject().toString());
                break;
            case INIT_FAILED:
                JSONObject json = new JSONObject();
                try {

                    json.put("msg", mInitMsg);

                } catch (Exception e) {
                    e.printStackTrace();
                }
                callUnityFunc("onInitFailed", new JSONObject().toString());
                break;

            default:
                break;
        }
        initState = INIT_DEFAULT;

    }

    public String getChannelName() {
        return AppConfig.getInstance().getConfigValue("qu" + "ic" + "ks" + "dk_cha" + "nnel_name");
    }


    public void requestCallFunctionWithParamsCallBack(int funcType, String[] data) {
        Log.d(TAG, "requestCallFunctionWithParamsCallBack: " + funcType + "\t" + data);


        Object[] objects = new Object[data.length];
        System.arraycopy(data, 0, objects, 0, data.length);
        int param0 = -111;
        try {
            param0 = Integer.valueOf(data[0]);
        }catch (Exception e){
            Log.e(TAG, "requestCallFunctionWithParamsCallBack: "+e.getMessage() );
        }

        if (param0 != -111){
            objects[0] = param0;
        }


        pringData(Arrays.stream(data).toArray());
        if (Extend.getInstance().isFunctionSupported(funcType)) {
            Extend.getInstance().callFunctionWithParamsCallBack(QuickUnityPlayerproxyActivity.this, funcType, new BaseCallBack() {
                @Override
                public void onSuccess(Object... objects) {

                    callUnityFunc("onBaseCallbackSuccess", objects.length > 0 ? objects[0].toString() : "success");
                }

                @Override
                public void onFailed(Object... objects) {
                    callUnityFunc("onBaseCallbackFaild", objects.length > 0 ? objects[0].toString() : "failded");
                }
            }, Arrays.stream(objects).toArray());
        } else {
            Log.d(TAG, "requestCallFunctionWithParamsCallBack: The funcType is not supported");
            callUnityFunc("onBaseCallbackFaild", "The funcType is not supported");
        }

    }

    private void pringData(Object... objects){
        for (Object o : objects){
            Log.d(TAG, "pringData: data == "+(String)o);
        }
    }


    public String getOaid() {
        String oaid = AppConfig.getInstance().getConfigValue("channel_oaid");
        Log.d(TAG, "oaid == " + oaid);
        return oaid;
    }

    public String getChannelVersion() {
        return AppConfig.getInstance().getChannelSdkVersion();
    }

    public int getChannelType() {
        return AppConfig.getInstance().getChannelType();
    }

    public String getSDKVersion() {
        return AppConfig.getInstance().getSdkVersion();
    }

    public String getConfigValue(String key) {
        return AppConfig.getInstance().getConfigValue(key);
    }

    public boolean isChannelHasExitDialog() {
        return QuickSDK.getInstance().isShowExitDialog();
    }

    public String getParentChannelType() {
        Log.d(TAG, "getParentChannelType " + Extend.getInstance().getParentChannelType());
        return Extend.getInstance().getParentChannelType() + "";
    }

    public void exitGame() {
        Log.d(TAG, "调用了exitGame()");
        this.finish();
        System.exit(0);
    }

    // ------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------
    // notifiers
    // ------------------------------------------------------------------------------------
    private class QuickUnityInitNotify implements InitNotifier {

        @Override
        public void onSuccess() {

            if (!TextUtils.isEmpty(gameObjectName)) {
                callUnityFunc("onInitSuccess", new JSONObject().toString());
            } else {
                initState = INIT_SUCCESS;
            }

        }

        @Override
        public void onFailed(String msg, String trace) {

            if (!TextUtils.isEmpty(gameObjectName)) {
                JSONObject json = new JSONObject();
                try {

                    json.put("msg", msg);

                } catch (Exception e) {
                    e.printStackTrace();
                }
                callUnityFunc("onInitFailed", new JSONObject().toString());
            } else {
                mInitMsg = msg;
                initState = INIT_FAILED;
            }

        }
    }

    private class QuickUnityLoginNotify implements LoginNotifier {

        @Override
        public void onSuccess(UserInfo userInfo) {

            JSONObject json = new JSONObject();
            try {

                json.put("userName", userInfo.getUserName() == null ? "" : userInfo.getUserName());
                json.put("userId", userInfo.getUID() == null ? "" : userInfo.getUID());
                json.put("userToken", userInfo.getToken() == null ? "" : userInfo.getToken());
                json.put("channelToken", userInfo.getChannelToken());

                json.put("msg", "success");

            } catch (Exception e) {
                e.printStackTrace();
            }
            callUnityFunc("onLoginSuccess", json.toString());
        }

        @Override
        public void onCancel() {
            JSONObject json = new JSONObject();
            try {

                json.put("msg", "cancel");

            } catch (Exception e) {
            }

            callUnityFunc("onLoginFailed", json.toString());
        }

        @Override
        public void onFailed(String msg, String trace) {

            JSONObject json = new JSONObject();
            try {

                json.put("msg", msg);

            } catch (Exception e) {
            }

            callUnityFunc("onLoginFailed", json.toString());
        }
    }

    private class QuickUnitySwitchAccountNotify implements SwitchAccountNotifier {

        @Override
        public void onCancel() {

        }

        @Override
        public void onFailed(String msg, String trace) {

        }

        @Override
        public void onSuccess(UserInfo userInfo) {
            Log.d(TAG, "切换账号成功");
            JSONObject json = new JSONObject();
            try {

                json.put("userName", userInfo.getUserName() == null ? "" : userInfo.getUserName());
                json.put("userId", userInfo.getUID() == null ? "" : userInfo.getUID());
                json.put("userToken", userInfo.getToken() == null ? "" : userInfo.getToken());
                json.put("channelToken", userInfo.getChannelToken());

                json.put("msg", "success");

            } catch (Exception e) {
                e.printStackTrace();
            }
            callUnityFunc("onSwitchAccountSuccess", json.toString());

        }

    }

    private class QuickUnityLogoutNotify implements LogoutNotifier {

        @Override
        public void onSuccess() {

            callUnityFunc("onLogoutSuccess", "success");
        }

        @Override
        public void onFailed(String msg, String trace) {

        }
    }

    private class QuickUnityPayNotify implements PayNotifier {

        @Override
        public void onSuccess(String quickOrderId, String cpOrderId, String extrasParams) {

            JSONObject json = new JSONObject();
            try {
                json.put("orderId", quickOrderId);
                json.put("cpOrderId", cpOrderId);
                json.put("extraParam", extrasParams);

            } catch (Exception e) {
            }

            callUnityFunc("onPaySuccess", json.toString());
        }

        @Override
        public void onCancel(String cpOrderID) {
            JSONObject json = new JSONObject();
            try {
                json.put("orderId", "");
                json.put("cpOrderId", cpOrderID);
                json.put("extraParam", "");

            } catch (Exception e) {
            }

            callUnityFunc("onPayCancel", json.toString());
        }

        @Override
        public void onFailed(String cpOrderID, String message, String trace) {
            JSONObject json = new JSONObject();
            try {
                json.put("orderId", "");
                json.put("cpOrderId", cpOrderID);
                json.put("extraParam", message);

            } catch (Exception e) {
            }

            callUnityFunc("onPayFailed", json.toString());
        }
    }

    private class QuickUnityExitNotiry implements ExitNotifier {

        @Override
        public void onSuccess() {
            callUnityFunc("onExitSuccess", "success");
        }

        @Override
        public void onFailed(String msg, String trace) {
        }
    }

    public void callUnityFunc(String funcName, String paramStr) {
        if (TextUtils.isEmpty(gameObjectName)) {
            Log.e(TAG, "gameObject is null, please set gameObject first");
            return;
        }

        UnityPlayer.UnitySendMessage(gameObjectName, funcName, paramStr);
    }

    @SuppressWarnings("unchecked")
    @Override
    public boolean handleMessage(Message msg) {
        switch (msg.what) {
            case MSG_LOGIN:

                Log.e(TAG, "login");
                User.getInstance().login(QuickUnityPlayerproxyActivity.this);
                break;
            case MSG_LOGOUT:

                Log.e(TAG, "logout");

                User.getInstance().logout(this);
                break;
            case MSG_PAY: {

                Log.e(TAG, "pay");

                HashMap<String, Object> mapObj = (HashMap<String, Object>) msg.obj;
                OrderInfo orderInfo = (OrderInfo) mapObj.get("orderInfo");
                GameRoleInfo roleInfo = (GameRoleInfo) mapObj.get("roleInfo");
                Payment.getInstance().pay(this, orderInfo, roleInfo);
                break;
            }
            case MSG_INIT: {
                Log.e(TAG, "init (unityreCall)");
                isLancScape = QuickUnityPlayerproxyActivity.this.getResources()
                        .getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE;
                QuickSDK.getInstance().setInitNotifier(initNotify).setLoginNotifier(loginNotify)
                        .setLogoutNotifier(logoutNotify).setPayNotifier(payNotify).setExitNotifier(exitNotiry)
                        .setIsLandScape(isLancScape).setSwitchAccountNotifier(switchAccountNotify);
                Sdk.getInstance().init(QuickUnityPlayerproxyActivity.this, getProductCode(), getProductKey());
                break;
            }
            case MSG_EXIT: {

                Log.e(TAG, "exit");

                Sdk.getInstance().exit(this);
                break;
            }
            case MSG_ROLEINFO: {

                Log.d(TAG, "update role info");

                GameRoleInfo roleInfo = (GameRoleInfo) msg.obj;
                boolean isCreate = (msg.arg1 == 1);

                User.getInstance().setGameRoleInfo(this, roleInfo, isCreate);
                break;
            }
            case MSG_EXTEND_FUNC: {
                int funcType = msg.arg1;
                Log.d(TAG, "handleMessage: MSG_EXTEND_FUNC funcType == "+funcType);
                if (funcType == MSG_EXTEND_FUNC_GET_DEVICE_ID) {
                    String deviceID = getDeviceID();
                    callUnityFunc("onSuccess", deviceID);
                } else if (funcType == FuncType.REAL_NAME_REGISTER) {
                    Log.d(TAG, "CallrealName:callFunctionWithParamsCallBack");
                    Extend.getInstance().callFunctionWithParamsCallBack(this, funcType, new BaseCallBack() {

                        @Override
                        public void onSuccess(Object... infos) {
                            Log.d(TAG, "CallrealName:callFunctionWithParamsCallBack:" + infos[0].toString());
                            JSONObject jsonObject = (JSONObject) infos[0];
                            try {
                                jsonObject.put("FunctionType", FuncType.REAL_NAME_REGISTER);
                                callUnityFunc("onSuccess", jsonObject.toString() + "");
                            } catch (JSONException e) {
                                Log.e(TAG, e.getMessage());
                            }
                        }

                        @Override
                        public void onFailed(Object... infos) {
                            String msg = (String) infos[0];
                            JSONObject jsonObject = new JSONObject();
                            try {
                                jsonObject.put("FunctionType", FuncType.REAL_NAME_REGISTER);
                                jsonObject.put("msg", msg);
                                callUnityFunc("onFail", jsonObject.toString());
                            } catch (JSONException e) {
                                Log.e(TAG, e.getMessage());
                            }

                        }
                    });
                } else {

                    String s = Extend.getInstance().callFunction(this, funcType);
                    JSONObject jsonObject = new JSONObject();
                    try {
                        jsonObject.put("FunctionType", funcType);
                        jsonObject.put("msg", s);
                        callUnityFunc("onSuccess", jsonObject.toString());
                    } catch (JSONException e) {
                        Log.e(TAG, e.getMessage());
                    }

                }
                break;
            }
            case MSG_EXTEND_FUNC_WiTH_PARAMS: {
                try {
                    Log.d(TAG, "call MSG_EXTEND_FUNC_WiTH_PARAMS");
                    int funcType = msg.arg1;
                    String[] info = (String[]) msg.obj;
                    Extend.getInstance().callFunctionWithParams(this, funcType, Arrays.stream(info).toArray());
                } catch (Exception e) {
                    Log.e(TAG, "MSG_EXTEND_FUNC_WiTH_PARAMS ERROR " + e.toString());
                }


                break;
            }
            case MSG_EXTEND_FUNC_SHARE: {
                Log.e(TAG, "call channel share");
                ShareInfo shareInfo = (ShareInfo) msg.obj;
                Extend.getInstance().callFunctionWithParams(this, FuncType.SHARE, shareInfo);
                break;
            }
            case MSG_EXTEND_FUNC_GOODSINFO: {
                Log.e(TAG, "call getGoodsInfo");
                String gids = (String) msg.obj;
                Log.e(TAG, "gids" + gids);
                ArrayList<String> gidsList = new ArrayList<String>();
                String[] gid = gids.split(",");
                if (gid != null) {
                    for (int i = 0; i < gid.length; i++) {

                        gidsList.add(gid[i]);
                    }
                    Log.e(TAG, "gidsList:" + gidsList);
                    if (gidsList.size() > 0) {
                        Extend.getInstance().callFunctionWithParamsCallBack(this, 111, new BaseCallBack() {

                            @Override
                            public void onSuccess(Object... infos) {
                                goodsInfos = null;
                                goodsInfos = new String((String) infos[0]);
                                Log.e(TAG, "goods info前:" + goodsInfos);
                                UnityPlayer.UnitySendMessage(gameObjectName, "onSuccess", goodsInfos);
                            }

                            @Override
                            public void onFailed(Object... infos) {
                                String msg = (String) infos[0];
                                callUnityFunc("onFail", msg + "");
                            }
                        }, gidsList);
                    }
                } else {
                    callUnityFunc("onFail", gids);
                }

                break;
            }

            default:
                break;
        }
        return false;
    }

    public abstract String getProductCode();

    public abstract String getProductKey();

}
