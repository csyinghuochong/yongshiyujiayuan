//
//  DYOpenSDKTrackerManager.h
//  AWEAnywhereArena
//
//  Created by ByteDance on 2025/2/18.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@protocol DYOpenSDKTrackerManagerDelegate <NSObject>

- (void)dyTrackerManagerEvent:(NSString * _Nonnull)event params:(NSDictionary * _Nonnull)params;

@end

@interface DYOpenSDKTrackerManager : NSObject

@property (nullable, nonatomic, strong) id<DYOpenSDKTrackerManagerDelegate> delegate;

+ (instancetype)shareManager;

/// 发送埋点·
/// - Parameters:
///   - event: 埋点名称（非空）
///   - params: 埋点参数（非空）
- (void)event:(NSString * _Nonnull)event params:(NSDictionary * _Nonnull)params;

- (void)getCurrentAllTrackerCountWithCompletion:(void(^)(NSInteger count))completion;

@end

NS_ASSUME_NONNULL_END
