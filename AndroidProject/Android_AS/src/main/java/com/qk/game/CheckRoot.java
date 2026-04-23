package com.qk.game;

import android.os.Build;
import android.util.Log;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.ArrayList;

/* JADX INFO: loaded from: classes.jar:com/guangying/yongshi/CheckRoot.class */
public class CheckRoot {
    private static String LOG_TAG = CheckRoot.class.getName();

    public static boolean isDeviceRooted() {
        if (checkDeviceDebuggable() || checkSuperuserApk() || checkBusybox() || checkAccessRootData() || checkGetRootAuth()) {
            return true;
        }
        return false;
    }

    public static boolean checkDeviceDebuggable() {
        String buildTags = Build.TAGS;
        if (buildTags != null && buildTags.contains("test-keys")) {
            Log.i(LOG_TAG, "buildTags=" + buildTags);
            return true;
        }
        return false;
    }

    public static boolean checkSuperuserApk() {
        try {
            File file = new File("/system/app/Superuser.apk");
            if (file.exists()) {
                Log.i(LOG_TAG, "/system/app/Superuser.apk exist");
                return true;
            }
            return false;
        } catch (Exception e) {
            return false;
        }
    }

    public static synchronized boolean checkGetRootAuth() {
        Process process = null;
        DataOutputStream os = null;
        try {
            try {
                Log.i(LOG_TAG, "to exec su");
                process = Runtime.getRuntime().exec("su");
                os = new DataOutputStream(process.getOutputStream());
                os.writeBytes("exit\n");
                os.flush();
                int exitValue = process.waitFor();
                Log.i(LOG_TAG, "exitValue=" + exitValue);
                if (exitValue == 0) {
                    if (os != null) {
                        try {
                            os.close();
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                    process.destroy();
                    return true;
                }
                if (os != null) {
                    try {
                        os.close();
                    } catch (Exception e2) {
                        e2.printStackTrace();
                    }
                }
                process.destroy();
                return false;
            } catch (Exception e3) {
                Log.i(LOG_TAG, "Unexpected error - Here is what I know: " + e3.getMessage());
                if (os != null) {
                    try {
                        os.close();
                    } catch (Exception e4) {
                        e4.printStackTrace();
                        return false;
                    }
                }
                process.destroy();
                return false;
            }
        } catch (Throwable th) {
            if (os != null) {
                try {
                    os.close();
                } catch (Exception e5) {
                    e5.printStackTrace();
                    throw th;
                }
            }
            process.destroy();
            throw th;
        }
    }

    public static synchronized boolean checkBusybox() {
        try {
            Log.i(LOG_TAG, "to exec busybox df");
            String[] strCmd = {"busybox", "df"};
            ArrayList<String> execResult = executeCommand(strCmd);
            if (execResult != null) {
                Log.i(LOG_TAG, "execResult=" + execResult.toString());
                return true;
            }
            Log.i(LOG_TAG, "execResult=null");
            return false;
        } catch (Exception e) {
            Log.i(LOG_TAG, "Unexpected error - Here is what I know: " + e.getMessage());
            return false;
        }
    }

    public static ArrayList<String> executeCommand(String[] shellCmd) {
        ArrayList<String> fullResponse = new ArrayList<>();
        try {
            Log.i(LOG_TAG, "to shell exec which for find su :");
            Process localProcess = Runtime.getRuntime().exec(shellCmd);
            new BufferedWriter(new OutputStreamWriter(localProcess.getOutputStream()));
            BufferedReader in = new BufferedReader(new InputStreamReader(localProcess.getInputStream()));
            while (true) {
                try {
                    String line = in.readLine();
                    if (line == null) {
                        break;
                    }
                    Log.i(LOG_TAG, "–> Line received: " + line);
                    fullResponse.add(line);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            Log.i(LOG_TAG, "–> Full response was: " + fullResponse);
            return fullResponse;
        } catch (Exception e2) {
            return null;
        }
    }

    public static synchronized boolean checkAccessRootData() {
        try {
            Log.i(LOG_TAG, "to write /data");
            Boolean writeFlag = writeFile("/data/su_test", "test_ok");
            if (writeFlag.booleanValue()) {
                Log.i(LOG_TAG, "write ok");
            } else {
                Log.i(LOG_TAG, "write failed");
            }
            Log.i(LOG_TAG, "to read /data");
            String strRead = readFile("/data/su_test");
            Log.i(LOG_TAG, "strRead=" + strRead);
            if ("test_ok".equals(strRead)) {
                return true;
            }
            return false;
        } catch (Exception e) {
            Log.i(LOG_TAG, "Unexpected error - Here is what I know: " + e.getMessage());
            return false;
        }
    }

    public static Boolean writeFile(String fileName, String message) {
        try {
            FileOutputStream fout = new FileOutputStream(fileName);
            byte[] bytes = message.getBytes();
            fout.write(bytes);
            fout.close();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    public static String readFile(String fileName) {
        File file = new File(fileName);
        try {
            FileInputStream fis = new FileInputStream(file);
            byte[] bytes = new byte[1024];
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            while (true) {
                int len = fis.read(bytes);
                if (len > 0) {
                    bos.write(bytes, 0, len);
                } else {
                    String result = new String(bos.toByteArray());
                    Log.i(LOG_TAG, result);
                    return result;
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }
}
