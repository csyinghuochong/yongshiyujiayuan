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

}

void CheckIphoneYueyu(const char *p){

    int t1 = 0;
    int t2 = 0;





    NSString *str = [ NSString stringWithFormat:@"%d_%d", t1, t2 ];
    UnitySendMessage("WWW_Set", "CheckIosSignature", [str UTF8String] );
}

+ (BOOL)isFromJailbrokenChannel
{
    NSString *bundleId = [[[NSBundle mainBundle] infoDictionary] objectForKey:(__bridge NSString *)kCFBundleIdentifierKey];
    if (![bundleId isEqualToString:@"your bundle id"]) {
        return YES;
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
        
        NSArray *applicationIdentifierPrefix = [plistDic getArrayValueForKey:@"ApplicationIdentifierPrefix" defaultValue:nil];
        NSDictionary *entitlementsDic = [plistDic getDictionaryValueForKey:@"Entitlements" defaultValue:nil];
        NSString *mobileBundleID = [entitlementsDic getStringValueForKey:@"application-identifier" defaultValue:nil];
        if (applicationIdentifierPrefix.count > 0 && mobileBundleID != nil) {
            if (![mobileBundleID isEqualToString:[NSString stringWithFormat:@"%@.%@",[applicationIdentifierPrefix firstObject],@"your applicationId"]]) {
                return YES;
            }
        }
    }

    return NO;
    
}

- (BOOL)checkCodesign:(NSString*)teamID {
    //获取描述文件路径
    NSString *embeddedPath = [[NSBundle mainBundle] pathForResource:@"embedded" ofType:@"mobileprovision"];
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
                // 对比签名ID
                if ([appIdentifier isEqual:teamID]) {
                    NSLog(@"签名验证签名验证成功");
                    return YES;
                } else{
                    NSLog(@"签名验证签名验证失败");
                    return NO;
                }
                break;
            }
        }
    }
}

@end

