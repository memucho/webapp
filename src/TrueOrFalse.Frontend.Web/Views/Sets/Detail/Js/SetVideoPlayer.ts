
var setVideo: SetVideo;                                               

declare var initPlayer: () => void;
declare var player;

class StopVideoAt {                                                     

    QuestionId: number;
    Seconds : number;

    constructor(questionId: number, seconds: number) {
        this.QuestionId = questionId;
        this.Seconds = seconds;
    }
}

class SetVideoPlayer                                                              
{
    VideoStops: StopVideoAt[] = [];                
    VideoIsPlaying = false;                          
    VideoCheckIntervalPaused = false;                

    IsVideoPausingEnabled = true;                    

    Player : YT.Player;

    constructor() {                                               
        initPlayer = () => {                                      
            player = new YT.Player('player', {               
                playerVars: { rel: 0 },
                events: {
                    'onReady': setVideoPlayer.OnPlayerReady,                   
                    'onStateChange': function (e) { setVideoPlayer.OnStateChange(e, setVideoPlayer) }       
                }                                                                                           
            });
        }
        apiLoad();

        $(() => {
            this.InitVideoStops();
            this.StartTimecodeCheck();
            new VideoPausingButtons(this);
        });
    }

    public OnPlayerReady() {                                       
        $(document).ready(() => {
            this.Player = player;
      
        });
    }

    public OnStateChange(event : YT.EventArgs, setVideoPlayer : SetVideoPlayer) {

        if (event.data == YT.PlayerState.PLAYING) {
            setVideoPlayer.VideoIsPlaying = true;
            this.VideoCheckIntervalPaused = false;
            $('#continue').hide();
        } else {
            setVideoPlayer.VideoIsPlaying = false;    
        }
    }

    public InitVideoStops() {
        var self = this;

        $("#video-pager a[data-video-question-id]").each(function () {                  

            var pauseAt = +$(this).attr("data-video-pause-at");                      

            if (pauseAt == 0)                                                          
                return;

            self.VideoStops.push(                                                  
                new StopVideoAt(                                                                                                                         
                    +$(this).attr("data-video-question-id"),                       
                    pauseAt                                                          
                ));
        });
      
    }

    public StartTimecodeCheck() {

        var lastVideoCheck = 0;
        var interval = setInterval(() => {

            if (!this.VideoIsPlaying || this.VideoCheckIntervalPaused || !this.IsVideoPausingEnabled)
                return;

            this.VideoCheckIntervalPaused = true;

            var currentTime = Math.round(player.getCurrentTime()) - 1;

            if (lastVideoCheck == currentTime) {
                this.VideoCheckIntervalPaused = false;
                return;
            }

            lastVideoCheck = currentTime;

            var stops: StopVideoAt[] = this.VideoStops.filter(item => item.Seconds == currentTime);

            if (stops.length > 0) {
               // arrayStops = stops;
                player.pauseVideo();
                setVideo.ShowYoutubeOverlay();
                SetVideo.ClickItem(stops[0].QuestionId);
            } else {
                this.VideoCheckIntervalPaused = false;
            }

        }, 500);
    }
    

}

class VideoPausingButtons {
    _stopPausingBtn: JQuery;
    _startPausingBtn: JQuery;

    _setVideoPlayer: SetVideoPlayer;

    constructor(setVideoPlayer : SetVideoPlayer) {
        this._setVideoPlayer = setVideoPlayer;
        this.InitPauseStopActions();
    }

    InitPauseStopActions() {
        this._stopPausingBtn = $("#stopPausingVideo");
        this._startPausingBtn = $("#startPausingVideo");

        if (this._setVideoPlayer.IsVideoPausingEnabled) {
            this.ShowStopPausingBtn();
        } else {
            this.ShowStartPausingBtn();
        }

        this._stopPausingBtn.click((e) => {
            e.preventDefault();
            this._setVideoPlayer.IsVideoPausingEnabled = false;
            this.ShowStartPausingBtn();
        });

        this._startPausingBtn.click((e) => {
            e.preventDefault();
            this._setVideoPlayer.IsVideoPausingEnabled = true;
            this.ShowStopPausingBtn();
        });
    }

    ShowStartPausingBtn() {
        this._startPausingBtn.show();
        this._stopPausingBtn.hide();
    }

    ShowStopPausingBtn() {
        this._startPausingBtn.hide();
        this._stopPausingBtn.show(); 
    }

}




var setVideoPlayer = new SetVideoPlayer();
