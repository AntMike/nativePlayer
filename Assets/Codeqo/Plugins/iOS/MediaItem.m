//
//  MediaItem.m
//  Receive MediaItem Information from Unity
//
//  Created by Yohan Song 5/10/22.
//  Copyright © 2022 Codeqo. All rights reserved.
//

#import "MediaItem.h"
#import "JsonUtility.h"
#import "SharedVariables.h"
#import "NativeMediaPlayer.h"

@interface MediaItem()
- (void) reset;
- (void) applyDefaultMediaMetadata;
- (void) resetDefaultMediaMetadata;
- (void) fillInEmptyMediaMetadata;
- (NSString*) addIndex: (NSString*)_data;
- (NSString*) getDefaultAlbumTitle;
- (NSString*) getDefaultAlbumArtist;
- (NSString*) getDefaultTitle;
- (NSString*) getDefaultArtist;
@end

@implementation MediaItem
@synthesize AlbumTitle;
@synthesize AlbumArtist;
@synthesize Title;
@synthesize Artist;
@synthesize Genre;
@synthesize ReleaseDate;
@synthesize ArtData;

- (id) initWithIndex: (int)_index {
	if(self = [super init]) {
        self.MediaItemIndex = _index;
        prepared = false;
        [self reset];
	}
	return self;
}

- (void) reset {
    [self resetDefaultMediaMetadata];
    self.Count = [[PlayerPrefs sharedObject] getInt:@"NUM_TRACKS"];
    self.MediaLocation = [[PlayerPrefs sharedObject] getInt:@"URI_TYPE"];
    retrieveArtworkType = [[PlayerPrefs sharedObject] getInt:@"TYPE_ARTWORK" id:self.MediaItemIndex];
    customMetadata = [[PlayerPrefs sharedObject] getInt:@"TYPE_METADATA" id:self.MediaItemIndex] == 1;

    NSString* path = [[PlayerPrefs sharedObject] getString:@"MEDIA_URI" id:self.MediaItemIndex];
    
    if (self.MediaLocation == STREAMING_ASSET) {
        NSString* fileName = path.stringByDeletingPathExtension;
        NSString* fileExt = path.pathExtension;
        path = [[NSBundle mainBundle] pathForResource:fileName ofType:fileExt inDirectory:@"Data/Raw"];
    }
    
    if (path == nil) {
        NSLog(@"Media doenst exist!");
        return;
    }
    
    self.MediaUri = [NSURL fileURLWithPath:path];

    if (customMetadata) [self loadCustomMediaMetadata];
    else [self retrieveMediaMetadata];
}

- (bool) isPrepared {
    NSLog(@"%@", prepared ? @"Item Prepared" : @"Item Not Prepared");
    return prepared;
}

- (void) loadCustomMediaMetadata {
    
    NSString *key = [NSMutableString stringWithFormat:@"Track%d", self.MediaItemIndex];
    NSDictionary *json = [[JsonUtility sharedObject] loadNSDictionaryFromJson:key];
    
    self.AlbumTitle = [json objectForKey:@"AlbumTitle"];
    self.AlbumArtist = [json objectForKey:@"AlbumArtist"];
    self.Title = [json objectForKey:@"Title"];
    self.Artist = [json objectForKey:@"Artist"];
    self.Genre = [json objectForKey:@"Genre"];
    self.ReleaseDate = [json objectForKey:@"ReleaseDate"];
    NSString *artData =[json objectForKey:@"ArtworkData"];
    if (artData) {
        self.ArtData = [[NSData alloc] initWithBase64EncodedString:artData
                                                           options:NSDataBase64DecodingIgnoreUnknownCharacters];
    }
    self.Artwork = [self getCustomArtwork];
    
    /*
    self.AlbumTitle = [[PlayerPrefs sharedObject] getString:@"ALBUM_TITLE" id:self.MediaItemIndex];
    self.AlbumArtist = [[PlayerPrefs sharedObject] getString:@"ALBUM_ARTIST" id:self.MediaItemIndex];
    self.Title = [[PlayerPrefs sharedObject] getString:@"TITLE" id:self.MediaItemIndex];
    self.Artist = [[PlayerPrefs sharedObject] getString:@"ARTIST" id:self.MediaItemIndex];
    self.Genre = [[PlayerPrefs sharedObject] getString:@"GENRE" id:self.MediaItemIndex];
    self.ReleaseDate = [[PlayerPrefs sharedObject] getString:@"RELEASE_DATE" id:self.MediaItemIndex];
    self.Artwork = [self getCustomArtwork];
     */
    
    [self fillInEmptyMediaMetadata];
    NSLog(@"%d.%@ : Custom Media Metadata Loaded", self.MediaItemIndex, self.Title);
}

- (void) retrieveMediaMetadata {
    /* Creating AVAasset to Retrieve Media Metadata */
    AVAsset *asset = [AVAsset assetWithURL:self.MediaUri];

    [asset loadValuesAsynchronouslyForKeys:@[@"commonMetadata"] completionHandler:^{
        NSError *error = nil;
        AVKeyValueStatus status = [asset statusOfValueForKey:@"commonMetadata" error:&error];

        switch (status) {
            case AVKeyValueStatusLoaded:
            {
                NSArray *artworks = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeyArtwork keySpace:AVMetadataKeySpaceCommon];
                NSArray *albumTitles = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeyAlbumName keySpace:AVMetadataKeySpaceCommon];
                NSArray *albumArtists = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeyAuthor keySpace:AVMetadataKeySpaceCommon];
                NSArray *titles = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeyTitle keySpace:AVMetadataKeySpaceCommon];
                NSArray *artists = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeyArtist keySpace:AVMetadataKeySpaceCommon];
                NSArray *genres = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeySubject keySpace:AVMetadataKeySpaceCommon];
                NSArray *releaseDates = [AVMetadataItem metadataItemsFromArray:asset.commonMetadata withKey:AVMetadataCommonKeyCreationDate keySpace:AVMetadataKeySpaceCommon];
                
                if (self->retrieveArtworkType == RETRIEVE_ARTWORK || self->retrieveArtworkType == ADD_CUSTOM_ARTWORK_WHEN_ARTWORK_IS_UNAVAILABLE) {
                      /* Retrieving Artwork */
                    for (AVMetadataItem* item in artworks) {
                        if ([item.keySpace isEqualToString:AVMetadataKeySpaceID3]) {
                            if (TARGET_OS_IPHONE && NSFoundationVersionNumber > NSFoundationVersionNumber_iOS_7_1) {
                                NSData *newImage = [item.value copyWithZone:nil];
                                self.ArtData = newImage;
                                self.Artwork = [self dataToMPMediaItemArtwork:newImage];
                            } else {
                                NSDictionary *dict = [item.value copyWithZone:nil];
                                if ([dict objectForKey:@"data"]) {
                                    self.ArtData = [dict objectForKey:@"data"];
                                    self.Artwork = [self dataToMPMediaItemArtwork:[dict objectForKey:@"data"]];
                                }
                            }
                        } else if ([item.keySpace isEqualToString:AVMetadataKeySpaceiTunes]) {
                            self.ArtData = [item.value copyWithZone:nil];
                            self.Artwork = [self dataToMPMediaItemArtwork:[item.value copyWithZone:nil]];
                        }
                    }
                    
                    if (self.Artwork == nil && self->retrieveArtworkType == ADD_CUSTOM_ARTWORK_WHEN_ARTWORK_IS_UNAVAILABLE) {
                        self->retrieveArtworkType = ADD_CUSTOM_ARTWORK;
                    }
                }
                
                if (self->retrieveArtworkType == ADD_CUSTOM_ARTWORK){
                    self.Artwork = [self getCustomArtwork];
                }

                self.AlbumTitle = [((AVMetadataItem*)[albumTitles firstObject]).value copyWithZone:nil];
                self.AlbumArtist = [((AVMetadataItem*)[albumArtists firstObject]).value copyWithZone:nil];
                self.Title = [((AVMetadataItem*)[titles firstObject]).value copyWithZone:nil];
                self.Artist = [((AVMetadataItem*)[artists firstObject]).value copyWithZone:nil];
                self.Genre = [((AVMetadataItem*)[genres firstObject]).value copyWithZone:nil];
                self.ReleaseDate = [((AVMetadataItem*)[releaseDates firstObject]).value copyWithZone:nil];

                NSLog(@"%d.%@ : Media Metadata Retrieved", self.MediaItemIndex, self.Title);
                
                [self fillInEmptyMediaMetadata];
                
                break;
            }
            case AVKeyValueStatusFailed:
                NSLog(@"Retrieving media metadata failed : Loading AVAsset failed");
                [self applyDefaultMediaMetadata];
                break;
            case AVKeyValueStatusCancelled:
                NSLog(@"Retrieving media metadata failed : Loading AVAsset cancelled");
                [self applyDefaultMediaMetadata];
                break;
            default:
                NSLog(@"Retrieving media metadata failed : Unknown");
                [self applyDefaultMediaMetadata];
                break;
        }
    }];
}

- (void) fillInEmptyMediaMetadata {
    if (self.AlbumTitle == nil)
        self.AlbumTitle = [self getDefaultAlbumTitle];
    if (self.AlbumArtist == nil)
        self.AlbumArtist = [self getDefaultAlbumArtist];
    if (self.Title == nil)
        self.Title = [self getDefaultTitle];
    if (self.Artist == nil)
        self.Artist = [self getDefaultArtist];
    if (self.Genre == nil)
        self.Genre = @"Unknown";
    if (self.ReleaseDate == nil)
        self.ReleaseDate = @"Unknown";
    
    prepared = true;
}

- (void) applyDefaultMediaMetadata {
    self.AlbumTitle = [self getDefaultAlbumTitle];
    self.AlbumArtist = [self getDefaultAlbumArtist];
    self.Title = [self getDefaultTitle];
    self.Artist = [self getDefaultArtist];
    self.Genre = @"Unknown";
    self.ReleaseDate = @"Unknown";
}

- (MPMediaItemArtwork*) dataToMPMediaItemArtwork: (NSData*)_data {
    UIImage *tempImg = [UIImage imageWithData:_data];
    return [[MPMediaItemArtwork alloc] initWithBoundsSize:CGSizeMake(600, 600) requestHandler: ^UIImage* _Nonnull(CGSize size) {
        return tempImg;
    }];
}

- (void) resetDefaultMediaMetadata {
    addIndexType = [[PlayerPrefs sharedObject] getInt:@"DEFAULT_ADD_INDEX_TYPE"];
    defaultAlbumTitle = [self addIndex:[[PlayerPrefs sharedObject] getString:@"DEFAULT_ALBUM"]];
    defaultAlbumArtist = [self addIndex:[[PlayerPrefs sharedObject] getString:@"DEFAULT_ALBUM_ARTIST"]];
    defaultTitle = [self addIndex:[[PlayerPrefs sharedObject] getString:@"DEFAULT_TITLE"]];
    defaultArtist = [self addIndex:[[PlayerPrefs sharedObject] getString:@"DEFAULT_ARTIST"]];
}

- (NSString*) addIndex: (NSString*)_data {
    if (addIndexType == ADD_INDEX_PREFIX) {
        return [NSString stringWithFormat:@"%d%@", (self.MediaItemIndex + 1), _data];
    } else if (addIndexType == ADD_INDEX_SUFFIX) {
        return [NSString stringWithFormat:@"%@%d", _data, (self.MediaItemIndex + 1)];
    } else {
        return _data;
    }
}

- (NSString*) getDefaultAlbumTitle {
    return defaultAlbumTitle;
}

- (NSString*) getDefaultAlbumArtist {
    return defaultAlbumArtist;
}

- (NSString*) getDefaultTitle {
    return defaultTitle;
}

- (NSString*) getDefaultArtist {
    return defaultArtist;
}

- (MPMediaItemArtwork*) getCustomArtwork {
    /* NSString *base64encoded = [[PlayerPrefs sharedObject] getString:@"ART" id:self.MediaItemIndex];
    if (base64encoded) {
        NSData *base64decoded = [[NSData alloc]
                                 initWithBase64EncodedString:base64encoded
                                                     options:NSDataBase64DecodingIgnoreUnknownCharacters];
        return [self dataToMPMediaItemArtwork:base64decoded];
    }*/
    if (self.ArtData) {
        return [self dataToMPMediaItemArtwork:self.ArtData];
    }
    return nil;
}

@end
