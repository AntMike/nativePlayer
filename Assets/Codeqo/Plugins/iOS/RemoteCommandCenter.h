//
//  NativeMediaPlayer.RemoteCommandCenter.h
//  RemoteCommandCenter Extention for NativeMediaPlayer for Unity 
//
//  Created by Yohan Song 5/10/22.
//  Copyright © 2022 Codeqo. All rights reserved.
//

#import <NativeMediaPlayer.h>
#import <AVFoundation/AVFoundation.h>
#import <AVKit/AVKit.h>

@interface RemoteCommandCenter : NSObject
{
    bool positionZero;
}

+ (RemoteCommandCenter*) sharedObject;
- (void) updateRemoteCommandCenter;

@end

typedef NS_ENUM(NSInteger, RemoteCommand) {
    Disabled,
    TogglePlayPause,
    Play,
    Pause,
    Stop,
    SkipToNext,
    SkipToPrevious,
    FastForward,
    Rewind
};
