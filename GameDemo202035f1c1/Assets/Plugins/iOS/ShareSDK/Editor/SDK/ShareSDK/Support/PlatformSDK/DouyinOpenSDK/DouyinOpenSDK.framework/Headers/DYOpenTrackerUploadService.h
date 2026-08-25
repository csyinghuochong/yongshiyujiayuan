//
//  DYOpenTrackerUploadService.h
//  Pods
//
//  Created by ByteDance on 2025/2/19.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@protocol DYOpenTrackerUploadServiceDelegate <NSObject>

// 埋点打包回调（多个埋点 -> 上传任务包）
- (void)dyTrackerUploadServicePackage:(nonnull NSMutableArray<NSDictionary *> *)trackerPackage withKey:(NSString *)key;

// 埋点发送状态回调
- (void)dyTrackerUploadStatusUpdateWithKey:(NSString *)key info:(NSDictionary *)info;

@end

@interface DYOpenTrackerUploadService : NSObject

@property (nullable, nonatomic, strong) id<DYOpenTrackerUploadServiceDelegate> delegate;

+ (nonnull instancetype)shareService;

/// 发送埋点·
/// - Parameters:
///   - event: 埋点名称（非空）
///   - params: 埋点参数（非空）
- (void)uploadTrackerPackage:(nonnull NSMutableArray<NSDictionary *> *)trackerPackage;

- (void)getCurrentAllTaskCountWithCompletion:(void(^)(NSInteger count))completion;

@end

NS_ASSUME_NONNULL_END
