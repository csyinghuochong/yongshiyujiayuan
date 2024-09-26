//
//  UJSInterface.m
//  Unity-iPhone
//
//  Created by MacMini on 14-5-15.
//
//

#import "IAPInterface.h"

@implementation IAPInterface


#define ARRAY_SIZE(a) sizeof(a)/sizeof(a[0])
 
const char* jailbreak_tool_pathes[] = {
    "/Applications/Cydia.app",
    "/Library/MobileSubstrate/MobileSubstrate.dylib",
    "/bin/bash",
    "/usr/sbin/sshd",
    "/etc/apt"
};
 

void CheckIphoneYueyu(const char *p){
    NSLog(@"TCheckIphoneYueyu!");
    int t1 = 0;
    int t2 = 0;
    int t3 = 0;
    
    for (int i=0; i<ARRAY_SIZE(jailbreak_tool_pathes); i++) {
        if ([[NSFileManager defaultManager] fileExistsAtPath:[NSString stringWithUTF8String:jailbreak_tool_pathes[i]]]) {
            NSLog(@"The device is jail broken!");
            t1 = 1;
        }
    }
    
    /// 根据是否能打开cydia判断
    if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"cydia://"]]) {
        t2 = 1;
    }
    /// 根据是否能获取所有应用的名称判断 没有越狱的设备是没有读取所有应用名称的权限的。
    if ([[NSFileManager defaultManager] fileExistsAtPath:@"User/Applications/"]) {
        t3 = 1;
    }
    
    
    NSString *str = [ NSString stringWithFormat:@"%d_%d_%d", t1, t2, t3 ];
    UnitySendMessage("WWW_Set", "OnRecvYueyu", [str UTF8String] );

    CheckIosSignature(p);
    
}

void CheckIosSignature(const char *p){
    NSLog(@"CheckIosSignature!");
    int t1 = [IAPInterface isFromJailbrokenChannel:@"com.guangying.yongshi" andString2:@"com.guangying.yongshi"];
    int t2 = [IAPInterface checkCodesign:@"3R4T6JWJ98"];


    NSString *str = [ NSString stringWithFormat:@"%d_%d", t1, t2 ];
    UnitySendMessage("WWW_Set", "OnRecvIosSignature", [str UTF8String] );
}

+(int)isFromJailbrokenChannel:(NSString*)appid andString2: (NSString*)initbundleid  {
    NSString *bundleId = [[[NSBundle mainBundle] infoDictionary] objectForKey:(__bridge NSString *)kCFBundleIdentifierKey];
    

    if (![bundleId isEqualToString:initbundleid]) {
        
        return 1;
    }
    

    //取出embedded.mobileprovision这个描述文件的内容进行判断
    NSString *mobileProvisionPath = [[NSBundle mainBundle] pathForResource:@"embedded" ofType:@"mobileprovision"];
    NSData *rawData = [NSData dataWithContentsOfFile:mobileProvisionPath];
    NSString *rawDataString = [[NSString alloc] initWithData:rawData encoding:NSASCIIStringEncoding];
    NSRange plistStartRange = [rawDataString rangeOfString:@"<plist"];
    NSRange plistEndRange = [rawDataString rangeOfString:@"</plist>"];
    if (plistStartRange.location != NSNotFound && plistEndRange.location != NSNotFound) {
       
       
        NSString *tempPlistString = [rawDataString substringWithRange:NSMakeRange(plistStartRange.location, NSMaxRange(plistEndRange))];
        NSData *tempPlistData = [tempPlistString dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *plistDic =  [NSPropertyListSerialization propertyListWithData:tempPlistData options:NSPropertyListImmutable format:nil error:nil];
        
        NSArray *applicationIdentifierPrefix = [plistDic objectForKey:@"ApplicationIdentifierPrefix" ];
        NSDictionary *entitlementsDic = [plistDic objectForKey:@"Entitlements" ];
        NSString *mobileBundleID = [entitlementsDic objectForKey:@"application-identifier"];
        
        NSLog(@"%@  bbbbbb1111", mobileBundleID);
        NSLog(@"%@  bbbbbb2222", [NSString stringWithFormat:@"%@.%@",[applicationIdentifierPrefix firstObject],initbundleid]);
        
        if (applicationIdentifierPrefix.count > 0 && mobileBundleID != nil) {
            if (![mobileBundleID isEqualToString:[NSString stringWithFormat:@"%@.%@",[applicationIdentifierPrefix firstObject],appid]]) {
                return 1;
            }
        }
    }
    
    return 0;
    
}

+(int)checkCodesign:(NSString*)initteamID {
    //获取描述文件路径
    NSString *embeddedPath = [[NSBundle mainBundle] pathForResource:@"embedded" ofType:@"mobileprovision"];
    
    
    NSLog(@"%@  ccccc11111", embeddedPath);
    if ([[NSFileManager defaultManager] fileExistsAtPath:embeddedPath]) {
        // 读取application-identifier
        NSString *embeddedProvisioning = [NSString stringWithContentsOfFile:embeddedPath encoding:NSASCIIStringEncoding error:nil];
        NSArray *embeddedProvisioningLines = [embeddedProvisioning componentsSeparatedByCharactersInSet:[NSCharacterSet newlineCharacterSet]];
        for (int i = 0; i < [embeddedProvisioningLines count]; i++) {
            if ([[embeddedProvisioningLines objectAtIndex:i] rangeOfString:@"application-identifier"].location != NSNotFound) {
                NSInteger fromPosition = [[embeddedProvisioningLines objectAtIndex:i+1] rangeOfString:@"<string>"].location+8;
                NSInteger toPosition = [[embeddedProvisioningLines objectAtIndex:i+1] rangeOfString:@"</string>"].location;
                NSRange range;
                range.location = fromPosition;
                range.length = toPosition - fromPosition;
                NSString *fullIdentifier = [[embeddedProvisioningLines objectAtIndex:i+1] substringWithRange:range];
                NSArray *identifierComponents = [fullIdentifier componentsSeparatedByString:@"."];
                NSString *appIdentifier = [identifierComponents firstObject];
                
                NSLog(@"%@  ddddd1111", initteamID);
                NSLog(@"%@  ddddd2222", appIdentifier);
                
                // 对比签名ID
                if ([appIdentifier isEqual:initteamID]) {
                    NSLog(@"签名验证签名验证成功");
                    return 0;
                } else{
                    NSLog(@"签名验证签名验证失败");
                    return 1;
                }
                break;
            }
        }
    }
    else {
        NSLog(@"%@  ccccc22222", embeddedPath);
        return 2;
    }
    return  0;;
}

@end

