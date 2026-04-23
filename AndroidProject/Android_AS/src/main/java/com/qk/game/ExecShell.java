package com.qk.game;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.ArrayList;

/* JADX INFO: loaded from: classes.jar:com/guangying/yongshi/ExecShell.class */
public class ExecShell {
    private static String LOG_TAG = ExecShell.class.getName();

    /* JADX INFO: loaded from: classes.jar:com/guangying/yongshi/ExecShell$SHELL_CMD.class */
    public enum SHELL_CMD {
        check_su_binary(new String[]{"/system/xbin/which", "su"});

        String[] command;

        SHELL_CMD(String[] command) {
            this.command = command;
        }
    }

    public ArrayList<String> executeCommand(SHELL_CMD shellCmd) {
        ArrayList<String> fullResponse = new ArrayList<>();
        try {
            Process localProcess = Runtime.getRuntime().exec(shellCmd.command);
            new BufferedWriter(new OutputStreamWriter(localProcess.getOutputStream()));
            BufferedReader in = new BufferedReader(new InputStreamReader(localProcess.getInputStream()));
            while (true) {
                try {
                    String line = in.readLine();
                    if (line == null) {
                        break;
                    }
                    fullResponse.add(line);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            return fullResponse;
        } catch (Exception e2) {
            return null;
        }
    }
}