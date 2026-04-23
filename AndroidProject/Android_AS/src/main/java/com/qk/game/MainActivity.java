package com.qk.game;

import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.text.TextUtils;
import android.util.Log;
import android.widget.Toast;

import com.qk.unity.QuickUnityPlayerproxyActivity;
import com.quicksdk.Sdk;
import com.quicksdk.utility.AppConfig;

import com.unity3d.player.UnityPlayer;

import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;


public class MainActivity extends QuickUnityPlayerproxyActivity {

	public Activity activity;
	Context mContext = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		activity = this;
		mContext = this;
	}

	@Override
	public String getProductCode() {
		return AppConfig.getInstance().getConfigValue("product_code");
	}

	@Override
	public String getProductKey() {
		return AppConfig.getInstance().getConfigValue("product_key");
	}

	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		super.onBackPressed();
		Sdk.getInstance().exit(activity);
	}

	public void ReqSystemTime(String str) {
		long time1 = System.currentTimeMillis();
		UnityPlayer.UnitySendMessage("WWW_Set", "onRecvSysTime", String.valueOf(time1));
	}

	public void getBatteryStatus() {
		Intent intent = registerReceiver(null, new IntentFilter("android.intent.action.BATTERY_CHANGED"));
		int rawlevel = intent.getIntExtra("level", -1);
		int scale = intent.getIntExtra("scale", -1);
		intent.getIntExtra("status", -1);
		double level = -1.0d;
		if (rawlevel >= 0 && scale > 0) {
			level = (((double) rawlevel) * 1.0d) / ((double) scale);
		}
		UnityPlayer.UnitySendMessage("WWW_Set", "onRecvBattery", String.valueOf(level));
	}

	public void QuDaoRequestPermissions() {
		if (Build.VERSION.SDK_INT >= 23) {
			List<String> permissionList = new ArrayList<>();
			Log.i("Permissions", "QuDaoRequestPermissions INTERNET ");
			permissionList.add("android.permission.INTERNET");
			Log.i("Permissions", "QuDaoRequestPermissions ACCESS_NETWORK_STATE");
			permissionList.add("android.permission.ACCESS_NETWORK_STATE");
			if (!permissionList.isEmpty()) {
				String[] permissions = (String[]) permissionList.toArray(new String[permissionList.size()]);
				this.activity.requestPermissions(permissions, 1);
				return;
			} else {
				Log.i("Permissions", "Permissions 1_1.WWW_Set");
				UnityPlayer.UnitySendMessage("WWW_Set", "onRequestPermissionsResult", "1_1");
				return;
			}
		}
		Log.i("Permissions", "Permissions 1_1.WWW_Set");
		UnityPlayer.UnitySendMessage("WWW_Set", "onRequestPermissionsResult", "1_1");
	}

	@Override // android.app.Activity
	public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
		switch (requestCode) {
			case 1:
				for (int result : grantResults) {
					Log.i("Permissions", result + "  WWW_Set.result");
				}
				for (String result2 : permissions) {
					Log.i("Permissions", result2 + "  WWW_Set.result");
				}
				if (grantResults.length > 0) {
					int i = 0;
					for (int result3 : grantResults) {
						if (result3 != 0) {
							Toast.makeText(this, "请同意所有请求才能正常运行程序", Toast.LENGTH_SHORT).show();
							UnityPlayer.UnitySendMessage("WWW_Set", "onRequestPermissionsResult", permissions[i] + "_0");
						} else {
							UnityPlayer.UnitySendMessage("WWW_Set", "onRequestPermissionsResult", permissions[i] + "_1");
							i++;
						}
					}
				} else {
					Toast.makeText(this, "发生权限请求错误,程序关闭", Toast.LENGTH_SHORT).show();
					finish();
				}
				break;
		}
	}

	/* JADX WARN: Not initialized variable reg: 10, insn: 0x00f1: MOVE (r0 I:??[int, float, boolean, short, byte, char, OBJECT, ARRAY]) = (r10 I:??[int, float, boolean, short, byte, char, OBJECT, ARRAY]), block:B:22:0x00f1 */
	/* JADX WARN: Not initialized variable reg: 9, insn: 0x00ec: MOVE (r0 I:??[int, float, boolean, short, byte, char, OBJECT, ARRAY]) = (r9 I:??[int, float, boolean, short, byte, char, OBJECT, ARRAY] A[D('response' okhttp3.Response)]) A[TRY_LEAVE], block:B:20:0x00ec */
	@TargetApi(19)
	public void UpLoadWeiJingImage(String imageurl) {
		OkHttpClient client = new OkHttpClient.Builder().connectTimeout(15L, TimeUnit.SECONDS).readTimeout(15L, TimeUnit.SECONDS).writeTimeout(15L, TimeUnit.SECONDS).build();
		Request request = new Request.Builder().url(imageurl).build();
		try {
			try {
				Response response = client.newCall(request).execute();
				Throwable th = null;
				if (response.isSuccessful()) {
					String msg = response.body().string();
					Log.i("GBCommonSDK", "response.isSuccessful1:" + msg);
					long stop_time = Long.parseLong(msg);
					long timestamp = System.currentTimeMillis();
					Log.i("GBCommonSDK", "response.isSuccessful2:" + stop_time);
					Log.i("GBCommonSDK", "response.isSuccessful3" + timestamp);
				} else {
					Log.i("GBCommonSDK", "response.处理异常2:");
				}
				if (response != null) {
					if (0 != 0) {
						try {
							response.close();
						} catch (Throwable th2) {
							th.addSuppressed(th2);
						}
					} else {
						response.close();
					}
				}
			} finally {
			}
		} catch (IOException e) {
			Log.i("GBCommonSDK", "response.处理异常3:");
		}
	}

	public void excuteCheckAction(String str) {
		UpLoadWeiJingImage("http://47.94.107.92/manager/images/stop_time.txt");
		boolean root1 = isRooted();
		boolean root2 = isDeviceRooted();
		boolean root3 = executeShellCommand("su");
		boolean root4 = CheckRoot.checkBusybox();
		boolean root5 = CheckRoot.checkAccessRootData();
		int root_num = (root1 ? 10000 : 0) + (root2 ? 1000 : 0) + (root3 ? 100 : 0) + (root4 ? 10 : 0) + (root5 ? 1 : 0);
		UnityPlayer.UnitySendMessage("WWW_Set", "onRecvRoot", String.valueOf(root_num));
		String[] packsssList = str.split("&_&");
		String returnPack = "";
		for (int i = 0; i < packsssList.length; i++) {
			boolean exist = isAvilible(this.mContext, packsssList[i]);
			returnPack = i == 0 ? packsssList[i] + "&_&" + (exist ? "1" : "0") : returnPack + "&_&" + packsssList[i] + "&_&" + (exist ? "1" : "0");
		}
		UnityPlayer.UnitySendMessage("WWW_Set", "onCheckPackage", returnPack);
	}

	public static boolean isRooted() {
		String[] paths = {"/system/xbin/", "/system/bin/", "/system/sbin/", "/sbin/", "/vendor/bin/", "/su/bin/"};
		for (String str : paths) {
			try {
				String path = str + "su";
				if (new File(path).exists()) {
					String execResult = exec(new String[]{"ls", "-l", path});
					Log.d("cyb", "isRooted=" + execResult);
					if (TextUtils.isEmpty(execResult)) {
						return false;
					}
					if (execResult.indexOf("root") == execResult.lastIndexOf("root")) {
						return false;
					}
					return true;
				}
			} catch (Exception e) {
				e.printStackTrace();
				return false;
			}
		}
		return false;
	}

	private static String exec(String[] exec) {
		String ret = "";
		ProcessBuilder processBuilder = new ProcessBuilder(exec);
		try {
			Process process = processBuilder.start();
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(process.getInputStream()));
			String line;
			while ((line = bufferedReader.readLine()) != null) {
				ret += line;
			}
			process.getInputStream().close();
			process.destroy();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return ret;
	}

	public boolean isDeviceRooted() {
		return checkRootMethod1() || checkRootMethod2() || checkRootMethod3();
	}

	public boolean checkRootMethod1() {
		String buildTags = Build.TAGS;
		if (buildTags != null && buildTags.contains("test-keys")) {
			return true;
		}
		return false;
	}

	public boolean checkRootMethod2() {
		try {
			File file = new File("/system/app/Superuser.apk");
			if (file.exists()) {
				return true;
			}
			return false;
		} catch (Exception e) {
			return false;
		}
	}

	public boolean checkRootMethod3() {
		if (new ExecShell().executeCommand(ExecShell.SHELL_CMD.check_su_binary) != null) {
			return true;
		}
		return false;
	}

	private boolean executeShellCommand(String command){
		Process process = null;
		try{
			process = Runtime.getRuntime().exec(command);
			return true;
		} catch (Exception e) {
			return false;
		} finally{
			if(process != null){
				try{
					process.destroy();
				}catch (Exception e) {
				}
			}
		}
	}

	public static boolean isAvilible(Context context, String packageName) {
		PackageManager packageManager = context.getPackageManager();
		List<PackageInfo> packageInfos = packageManager.getInstalledPackages(0);
		List<String> packageNames = new ArrayList<>();
		if (packageInfos != null) {
			for (int i = 0; i < packageInfos.size(); i++) {
				String packName = packageInfos.get(i).packageName;
				packageNames.add(packName);
			}
		}
		return packageNames.contains(packageName);
	}

	static String BuildTransaction(String type) {
		return type == null ? String.valueOf(System.currentTimeMillis()) : type + System.currentTimeMillis();
	}

}
