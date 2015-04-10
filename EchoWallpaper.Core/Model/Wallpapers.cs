using System;
using PropertyChanged;

namespace EchoWallpaper.Core.Model
{
    [ImplementPropertyChanged]
    public class Wallpapers
    {
        public string Title { get; set; }
        public Uri LandscapePreviewImage { get; set; }
        public Uri PortraitPreviewImage { get; set; }
        public Uri NineteenTwentyTwelveHundred { get; set; }
        public Uri NineteenTwentyTenEighty { get; set; }
        public Uri TwelveEightyTenTwentyFour { get; set; }
        public Uri TenTwentyFourSevenSixtyEight { get; set; }
        public Uri TwelveEightySevenSixtyEight { get; set; }
        public Uri TwelveEightySevenTwenty { get; set; }
        public Uri TwentyFiveSixtyFourteenForty { get; set; }

        public Uri IpadLandscape { get; set; }
        public Uri IpadLandscapeNoCalendar { get; set; }
        public Uri IpadPortrait { get; set; }
        public Uri IpadPortraitNoCalendar { get; set; }
        public Uri IpadMiniNoCalendar { get; set; }
        public Uri AndroidLandscape { get; set; }
        public Uri HdNoCalendar { get; set; }
        public Uri IphoneNoCalendar { get; set; }
        public Uri Wvga { get; set; }
        public Uri Wxga { get; set; }
        public Uri SevenTwentyP { get; set; }
    }
}
