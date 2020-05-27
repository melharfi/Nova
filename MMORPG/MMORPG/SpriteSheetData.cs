using MELHARFI.Gfx;
using System.Drawing;
using MELHARFI;

namespace MMORPG
{
    public class SpriteSheetData
    {
        public static SpriteSheetData SSD = new SpriteSheetData();

        public void Run()
        {
            // pour executer le singleton SSD
        }
        public SpriteSheetData()
        {
            // enregistrement des offset x,y,width,height des spritesheet

            #region /////////// classe 1 Naruto
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 0, new Rectangle(new Point(0, 0), new Size(25, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 1, new Rectangle(new Point(33, 0), new Size(23, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 2, new Rectangle(new Point(66, 0), new Size(23, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 3, new Rectangle(new Point(99, 0), new Size(23, 54)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 4, new Rectangle(new Point(4, 57), new Size(18, 56)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 5, new Rectangle(new Point(35, 59), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 6, new Rectangle(new Point(69, 58), new Size(18, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 7, new Rectangle(new Point(99, 59), new Size(21, 54)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 8, new Rectangle(new Point(4, 119), new Size(18, 56)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 9, new Rectangle(new Point(35, 121), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 10, new Rectangle(new Point(68, 120), new Size(18, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 11, new Rectangle(new Point(99, 121), new Size(21, 54)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 12, new Rectangle(new Point(1, 183), new Size(25, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 13, new Rectangle(new Point(35, 184), new Size(22, 52)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 14, new Rectangle(new Point(66, 183), new Size(23, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), 15, new Rectangle(new Point(98, 184), new Size(22, 52)));
            #endregion//////////////////////////////////////////////////////////////////////////////////////////////

            #region////////////// classe 2 Choji
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 0, new Rectangle(new Point(3, 1), new Size(25, 57)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 1, new Rectangle(new Point(35, 1), new Size(25, 57)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 2, new Rectangle(new Point(69, 1), new Size(25, 57)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 3, new Rectangle(new Point(101, 0), new Size(25, 58)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 4, new Rectangle(new Point(5, 64), new Size(22, 60)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 5, new Rectangle(new Point(32, 66), new Size(26, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 6, new Rectangle(new Point(68, 64), new Size(22, 60)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 7, new Rectangle(new Point(100, 65), new Size(23, 59)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 8, new Rectangle(new Point(0, 130), new Size(22, 60)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 9, new Rectangle(new Point(33, 131), new Size(23, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 10, new Rectangle(new Point(66, 130), new Size(22, 60)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 11, new Rectangle(new Point(98, 132), new Size(26, 58)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 12, new Rectangle(new Point(1, 200), new Size(25, 56)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 13, new Rectangle(new Point(33, 201), new Size(26, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 14, new Rectangle(new Point(67, 200), new Size(25, 56)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.choji.ToString(), 15, new Rectangle(new Point(99, 201), new Size(26, 55)));
            #endregion//////////////////////////////////////////////////////////////////////////////////////////////
            
            #region ///////// classe 3 Kabuto
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 0, new Rectangle(new Point(0, 0), new Size(23, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 1, new Rectangle(new Point(45, 0), new Size(21, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 2, new Rectangle(new Point(91, 1), new Size(23, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 3, new Rectangle(new Point(136, 1), new Size(22, 53)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 4, new Rectangle(new Point(9, 61), new Size(17, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 5, new Rectangle(new Point(49, 62), new Size(23, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 6, new Rectangle(new Point(99, 61), new Size(17, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 7, new Rectangle(new Point(140, 62), new Size(24, 53)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 8, new Rectangle(new Point(8, 125), new Size(17, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 9, new Rectangle(new Point(47, 126), new Size(24, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 10, new Rectangle(new Point(98, 124), new Size(17, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 11, new Rectangle(new Point(137, 125), new Size(23, 53)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 12, new Rectangle(new Point(1, 191), new Size(22, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 13, new Rectangle(new Point(48, 192), new Size(21, 52)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 14, new Rectangle(new Point(90, 191), new Size(22, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kabuto.ToString(), 15, new Rectangle(new Point(135, 193), new Size(22, 51)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////

            #region ////////// classe 4 ino
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 0, new Rectangle(new Point(2, 0), new Size(21, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 1, new Rectangle(new Point(37, 0), new Size(20, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 2, new Rectangle(new Point(70, 0), new Size(21, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 3, new Rectangle(new Point(103, 0), new Size(20, 53)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 4, new Rectangle(new Point(4, 64), new Size(19, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 5, new Rectangle(new Point(37, 66), new Size(22, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 6, new Rectangle(new Point(70, 64), new Size(19, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 7, new Rectangle(new Point(98, 66), new Size(25, 53)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 8, new Rectangle(new Point(0, 130), new Size(18, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 9, new Rectangle(new Point(30, 132), new Size(22, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 10, new Rectangle(new Point(65, 130), new Size(18, 55)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 11, new Rectangle(new Point(95, 132), new Size(26, 53)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 12, new Rectangle(new Point(0, 199), new Size(21, 52)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 13, new Rectangle(new Point(34, 200), new Size(20, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 14, new Rectangle(new Point(66, 199), new Size(21, 52)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.ino.ToString(), 15, new Rectangle(new Point(100, 200), new Size(20, 51)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////

            #region ///////////// classe 5 lee
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 0, new Rectangle(new Point(2, 0), new Size(23, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 1, new Rectangle(new Point(35, 0), new Size(23, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 2, new Rectangle(new Point(68, 0), new Size(23, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 3, new Rectangle(new Point(102, 0), new Size(21, 51)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 4, new Rectangle(new Point(7, 60), new Size(13, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 5, new Rectangle(new Point(37, 62), new Size(21, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 6, new Rectangle(new Point(72, 60), new Size(13, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 7, new Rectangle(new Point(101, 61), new Size(21, 52)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 8, new Rectangle(new Point(5, 121), new Size(13, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 9, new Rectangle(new Point(35, 122), new Size(21, 52)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 10, new Rectangle(new Point(70, 121), new Size(13, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 11, new Rectangle(new Point(101, 123), new Size(21, 51)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 12, new Rectangle(new Point(0, 185), new Size(23, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 13, new Rectangle(new Point(33, 186), new Size(21, 50)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 14, new Rectangle(new Point(65, 185), new Size(23, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.lee.ToString(), 15, new Rectangle(new Point(101, 185), new Size(21, 50)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////

            #region ///////////// classe 6 kanku
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 0, new Rectangle(new Point(0, 1), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 1, new Rectangle(new Point(31, 1), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 2, new Rectangle(new Point(60, 0), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 3, new Rectangle(new Point(90, 0), new Size(21, 54)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 4, new Rectangle(new Point(2, 62), new Size(17, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 5, new Rectangle(new Point(29, 62), new Size(21, 57)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 6, new Rectangle(new Point(64, 62), new Size(17, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 7, new Rectangle(new Point(89, 62), new Size(22, 56)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 8, new Rectangle(new Point(2, 129), new Size(17, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 9, new Rectangle(new Point(30, 130), new Size(22, 56)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 10, new Rectangle(new Point(64, 128), new Size(17, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 11, new Rectangle(new Point(97, 128), new Size(24, 57)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 12, new Rectangle(new Point(1, 194), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 13, new Rectangle(new Point(32, 195), new Size(21, 52)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 14, new Rectangle(new Point(63, 193), new Size(21, 54)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.kankura.ToString(), 15, new Rectangle(new Point(93, 193), new Size(21, 53)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////

            #region ///////////// classe 7 shikamaru
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 0, new Rectangle(new Point(2, 0), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 1, new Rectangle(new Point(34, 0), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 2, new Rectangle(new Point(69, 0), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 3, new Rectangle(new Point(101, 0), new Size(21, 59)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 4, new Rectangle(new Point(3, 67), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 5, new Rectangle(new Point(29, 66), new Size(27, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 6, new Rectangle(new Point(71, 66), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 7, new Rectangle(new Point(98, 65), new Size(26, 59)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 8, new Rectangle(new Point(0, 132), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 9, new Rectangle(new Point(30, 132), new Size(27, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 10, new Rectangle(new Point(63, 132), new Size(21, 59)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 11, new Rectangle(new Point(96, 132), new Size(26, 59)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 12, new Rectangle(new Point(2, 199), new Size(21, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 13, new Rectangle(new Point(34, 200), new Size(21, 57)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 14, new Rectangle(new Point(68, 199), new Size(21, 58)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.shikamaru.ToString(), 15, new Rectangle(new Point(100, 200), new Size(21, 57)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////

            #region ///////////// classe 8 sakura
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 0, new Rectangle(new Point(0, 0), new Size(21, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 1, new Rectangle(new Point(32, 0), new Size(20, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 2, new Rectangle(new Point(67, 0), new Size(21, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 3, new Rectangle(new Point(100, 0), new Size(20, 51)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 4, new Rectangle(new Point(3, 60), new Size(17, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 5, new Rectangle(new Point(30, 62), new Size(24, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 6, new Rectangle(new Point(69, 60), new Size(17, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 7, new Rectangle(new Point(98, 62), new Size(21, 52)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 8, new Rectangle(new Point(1, 121), new Size(17, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 9, new Rectangle(new Point(27, 123), new Size(24, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 10, new Rectangle(new Point(63, 121), new Size(17, 53)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 11, new Rectangle(new Point(95, 122), new Size(21, 52)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 12, new Rectangle(new Point(0, 185), new Size(21, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 13, new Rectangle(new Point(31, 186), new Size(21, 50)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 14, new Rectangle(new Point(63, 185), new Size(21, 51)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.sakura.ToString(), 15, new Rectangle(new Point(95, 186), new Size(21, 50)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////

            #region ///////////// classe 9 ikuka
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 0, new Rectangle(new Point(3, 0), new Size(25, 62)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 1, new Rectangle(new Point(37, 0), new Size(24, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 2, new Rectangle(new Point(68, 0), new Size(25, 62)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 3, new Rectangle(new Point(101, 0), new Size(24, 64)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 4, new Rectangle(new Point(4, 65), new Size(21, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 5, new Rectangle(new Point(34, 65), new Size(27, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 6, new Rectangle(new Point(71, 65), new Size(21, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 7, new Rectangle(new Point(98, 65), new Size(26, 63)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 8, new Rectangle(new Point(0, 131), new Size(21, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 9, new Rectangle(new Point(32, 130), new Size(27, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 10, new Rectangle(new Point(66, 130), new Size(21, 63)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 11, new Rectangle(new Point(99, 130), new Size(26, 63)));

            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 12, new Rectangle(new Point(2, 198), new Size(25, 61)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 13, new Rectangle(new Point(35, 198), new Size(24, 61)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 14, new Rectangle(new Point(67, 197), new Size(25, 61)));
            SpriteSheet.SetSpriteSheet(Enums.ActorClass.ClassName.iruka.ToString(), 15, new Rectangle(new Point(100, 196), new Size(24, 61)));
            #endregion //////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////  avatar naruto 
            SpriteSheet.SetSpriteSheet("avatar_" + Enums.ActorClass.ClassName.naruto, 0, new Rectangle(new Point(0, 0), new Size(25, 25)));       // thumb du visage classe
            SpriteSheet.SetSpriteSheet("avatar_" + Enums.ActorClass.ClassName.choji, 0, new Rectangle(new Point(3, 0), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("avatar_" + Enums.ActorClass.ClassName.kabuto, 0, new Rectangle(new Point(0, 0), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("avatar_" + Enums.ActorClass.ClassName.ino, 0, new Rectangle(new Point(0, 0), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("avatar_" + Enums.ActorClass.ClassName.iruka, 0, new Rectangle(new Point(3, 0), new Size(25, 25)));
            //////////////////////////////////////////////////////////////////////////////////////////////

            #region /////////////////////////// gfx/general/obj/1/all1.dat
            SpriteSheet.SetSpriteSheet("_Main_option", 0, new Rectangle(new Point(0, 2), new Size(15, 15)));            // btn option non enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 1, new Rectangle(new Point(16, 2), new Size(15, 15)));           // btn option non enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 2, new Rectangle(new Point(33, 3), new Size(30, 11)));           // image de deconnexion
            SpriteSheet.SetSpriteSheet("_Main_option", 3, new Rectangle(new Point(65, 2), new Size(15, 15)));           // btn fermer non enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 4, new Rectangle(new Point(82, 2), new Size(15, 15)));           // btn fermer en survole
            SpriteSheet.SetSpriteSheet("_Main_option", 5, new Rectangle(new Point(0, 18), new Size(210, 20)));          // btn rouge non enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 6, new Rectangle(new Point(0, 39), new Size(210, 20)));          // btn rouge enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 7, new Rectangle(new Point(100, 4), new Size(10, 10)));          // btn link
            SpriteSheet.SetSpriteSheet("_Main_option", 8, new Rectangle(new Point(114, 3), new Size(10, 13)));          // btn valider le chat
            SpriteSheet.SetSpriteSheet("_Main_option", 9, new Rectangle(new Point(1, 61), new Size(157, 35)));          // btn bleu non enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 10, new Rectangle(new Point(2, 98), new Size(157, 35)));         // btn bleu enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 11, new Rectangle(new Point(127, 4), new Size(12, 11)));         // image triangle qui s'affiche en bas de Chatbulle
            SpriteSheet.SetSpriteSheet("_Main_option", 12, new Rectangle(new Point(160, 62), new Size(34, 72)));        // gauge de santé
            SpriteSheet.SetSpriteSheet("_Main_option", 13, new Rectangle(new Point(140, 1), new Size(15, 15)));         // image des stats
            SpriteSheet.SetSpriteSheet("_Main_option", 14, new Rectangle(new Point(0, 135), new Size(180, 14)));        // gaugeTerreLvl
            SpriteSheet.SetSpriteSheet("_Main_option", 15, new Rectangle(new Point(0, 150), new Size(180, 14)));        // gaugeFeuLvl
            SpriteSheet.SetSpriteSheet("_Main_option", 16, new Rectangle(new Point(0, 165), new Size(180, 14)));        // gaugeVentLvl
            SpriteSheet.SetSpriteSheet("_Main_option", 17, new Rectangle(new Point(0, 180), new Size(180, 14)));        // gaugeFoudreLvl
            SpriteSheet.SetSpriteSheet("_Main_option", 18, new Rectangle(new Point(0, 195), new Size(180, 14)));        // gaugeTerreLvl
            SpriteSheet.SetSpriteSheet("_Main_option", 19, new Rectangle(new Point(200, 62), new Size(92, 22)));        // bouton blanc non enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 20, new Rectangle(new Point(200, 87), new Size(92, 22)));        // bouton blanc enfancé
            SpriteSheet.SetSpriteSheet("_Main_option", 21, new Rectangle(new Point(211, 0), new Size(40, 46)));         // cadre qui affiche le joueur lors du combat
            SpriteSheet.SetSpriteSheet("_Main_option", 22, new Rectangle(new Point(255, 0), new Size(40, 46)));         // cadre qui affiche le joueur lors du combat
            SpriteSheet.SetSpriteSheet("_Main_option", 23, new Rectangle(new Point(184, 137), new Size(49, 67)));       // chronometre pour le lancement du combat
            SpriteSheet.SetSpriteSheet("_Main_option", 24, new Rectangle(new Point(235, 151), new Size(48, 55)));       // cadre qui encercle l'avatar du joueur qui dois jouer dans le combat
            SpriteSheet.SetSpriteSheet("_Main_option", 25, new Rectangle(new Point(198, 113), new Size(84, 21)));       // boutton pour passer la main dans un combat
            SpriteSheet.SetSpriteSheet("_Main_option", 26, new Rectangle(new Point(2, 211), new Size(22, 19)));         // boutton pour quiter le combat
            SpriteSheet.SetSpriteSheet("_Main_option", 27, new Rectangle(new Point(28, 214), new Size(9, 12)));         // pc
            SpriteSheet.SetSpriteSheet("_Main_option", 28, new Rectangle(new Point(41, 214), new Size(16, 11)));        // pm
            SpriteSheet.SetSpriteSheet("_Main_option", 29, new Rectangle(new Point(61, 217), new Size(15, 8)));         // pe
            SpriteSheet.SetSpriteSheet("_Main_option", 30, new Rectangle(new Point(81, 212), new Size(4, 15)));         // cd
            SpriteSheet.SetSpriteSheet("_Main_option", 31, new Rectangle(new Point(89, 214), new Size(15, 12)));        // invoc
            SpriteSheet.SetSpriteSheet("_Main_option", 32, new Rectangle(new Point(108, 215), new Size(15, 10)));       // initiative
            SpriteSheet.SetSpriteSheet("_Main_option", 33, new Rectangle(new Point(129, 211), new Size(18, 18)));       // doton
            SpriteSheet.SetSpriteSheet("_Main_option", 34, new Rectangle(new Point(152, 211), new Size(18, 18)));       // katon
            SpriteSheet.SetSpriteSheet("_Main_option", 35, new Rectangle(new Point(176, 211), new Size(18, 18)));       // futon
            SpriteSheet.SetSpriteSheet("_Main_option", 36, new Rectangle(new Point(200, 211), new Size(18, 18)));       // raiton
            SpriteSheet.SetSpriteSheet("_Main_option", 37, new Rectangle(new Point(224, 211), new Size(18, 18)));       // suiton
            SpriteSheet.SetSpriteSheet("_Main_option", 38, new Rectangle(new Point(236, 137), new Size(8, 7)));         // coeur de pdv
            SpriteSheet.SetSpriteSheet("_Main_option", 39, new Rectangle(new Point(267, 136), new Size(16, 15)));       // flech qui montre le joueur qui a la main
            SpriteSheet.SetSpriteSheet("_Main_option", 40, new Rectangle(new Point(244, 214), new Size(10, 13)));       // bobo doton
            SpriteSheet.SetSpriteSheet("_Main_option", 41, new Rectangle(new Point(255, 214), new Size(10, 13)));       // bobo katon
            SpriteSheet.SetSpriteSheet("_Main_option", 42, new Rectangle(new Point(266, 214), new Size(10, 13)));       // bobo futon
            SpriteSheet.SetSpriteSheet("_Main_option", 43, new Rectangle(new Point(277, 214), new Size(10, 13)));       // bobo raiton
            SpriteSheet.SetSpriteSheet("_Main_option", 44, new Rectangle(new Point(288, 214), new Size(10, 13)));       // bobo suiton
            SpriteSheet.SetSpriteSheet("_Main_option", 45, new Rectangle(new Point(2, 233), new Size(17, 10)));        // esquivePC
            SpriteSheet.SetSpriteSheet("_Main_option", 46, new Rectangle(new Point(28, 232), new Size(23, 11)));       // esquivePM
            SpriteSheet.SetSpriteSheet("_Main_option", 47, new Rectangle(new Point(55, 233), new Size(22, 10)));       // esquivePE
            SpriteSheet.SetSpriteSheet("_Main_option", 48, new Rectangle(new Point(84, 228), new Size(10, 15)));       // esquiveCD
            SpriteSheet.SetSpriteSheet("_Main_option", 49, new Rectangle(new Point(101, 232), new Size(15, 11)));       // retraitPC
            SpriteSheet.SetSpriteSheet("_Main_option", 50, new Rectangle(new Point(124, 232), new Size(21, 11)));       // retraitPM
            SpriteSheet.SetSpriteSheet("_Main_option", 51, new Rectangle(new Point(153, 232), new Size(18, 10)));       // retraitPE
            SpriteSheet.SetSpriteSheet("_Main_option", 52, new Rectangle(new Point(176, 231), new Size(12, 15)));       // retraitCD
            SpriteSheet.SetSpriteSheet("_Main_option", 53, new Rectangle(new Point(194, 231), new Size(15, 13)));       // evasion
            SpriteSheet.SetSpriteSheet("_Main_option", 54, new Rectangle(new Point(215, 231), new Size(15, 13)));       // blocage
            SpriteSheet.SetSpriteSheet("_Main_option", 55, new Rectangle(new Point(286, 118), new Size(9, 88)));       // battle bare de déplacement
            SpriteSheet.SetSpriteSheet("_Main_option", 56, new Rectangle(new Point(246, 136), new Size(9, 9)));       // btn dock up dans la barre battle bare de déplacement
            SpriteSheet.SetSpriteSheet("_Main_option", 57, new Rectangle(new Point(257, 137), new Size(9, 8)));       // btn dock down dans la barre battle bare de déplacement
            SpriteSheet.SetSpriteSheet("_Main_option", 58, new Rectangle(new Point(3, 249), new Size(18, 18)));       // tete de mort
            SpriteSheet.SetSpriteSheet("_Main_option", 59, new Rectangle(new Point(26, 249), new Size(33, 18)));       // couronne en or pour un gagon qui na pas mort dans un combat
            SpriteSheet.SetSpriteSheet("_Main_option", 60, new Rectangle(new Point(63, 249), new Size(33, 18)));       // couronne en or pour un gagon qui est mort dans un combat
            SpriteSheet.SetSpriteSheet("_Main_option", 61, new Rectangle(new Point(99, 251), new Size(15, 15)));       // cadnas qui se met en haut de la tete d'un joueur lors d'un combat et en mode initialisation
            SpriteSheet.SetSpriteSheet("_Main_option", 62, new Rectangle(new Point(130, 247), new Size(50, 44)));       // image du coup dangeureux affiché au dessus du personnage
            SpriteSheet.SetSpriteSheet("_Main_option", 63, new Rectangle(new Point(206, 246), new Size(22, 21)));       // trangle au dessus du sort de l'envoutement pour afficher combien de tour il en reste
            SpriteSheet.SetSpriteSheet("_Main_option", 64, new Rectangle(new Point(0, 267), new Size(35, 33)));         // etoile des pc dans le hud
            SpriteSheet.SetSpriteSheet("_Main_option", 65, new Rectangle(new Point(35, 269), new Size(37, 31)));         // etoile des pc dans le hud
            SpriteSheet.SetSpriteSheet("_Main_option", 67, new Rectangle(new Point(160, 1), new Size(15, 16)));         // dom fix
            SpriteSheet.SetSpriteSheet("_Main_option", 68, new Rectangle(new Point(185, 250), new Size(15, 17)));         // puissance
            SpriteSheet.SetSpriteSheet("_Main_option", 69, new Rectangle(new Point(213, 49), new Size(8, 12)));         // precedant "fleche" pour faire passer les messages
            SpriteSheet.SetSpriteSheet("_Main_option", 70, new Rectangle(new Point(222, 49), new Size(8, 12)));         // suivant "fleche pour faire passer les messages
            SpriteSheet.SetSpriteSheet("_Main_option", 71, new Rectangle(new Point(299, 4), new Size(70, 84)));         // flech pour indiquer des emplacements
            SpriteSheet.SetSpriteSheet("_Main_option", 72, new Rectangle(new Point(116, 254), new Size(11, 11)));         // bouton fermer sur les bulles des pnj
            SpriteSheet.SetSpriteSheet("_Main_option", 73, new Rectangle(new Point(298, 92), new Size(15, 23)));         // menu sort
            SpriteSheet.SetSpriteSheet("_Main_option", 74, new Rectangle(new Point(4, 307), new Size(252, 42)));         // btn sort sur le menu des sorts activé
            SpriteSheet.SetSpriteSheet("_Main_option", 75, new Rectangle(new Point(3, 351), new Size(252, 42)));         // btn sort sur le menu des sorts désactivé
            SpriteSheet.SetSpriteSheet("_Main_option", 76, new Rectangle(new Point(234, 48), new Size(12, 13)));         // btn pour augementer un sort non cliqué
            SpriteSheet.SetSpriteSheet("_Main_option", 77, new Rectangle(new Point(248, 48), new Size(12, 13)));         // btn pour augementer un sort cliqué
            SpriteSheet.SetSpriteSheet("_Main_option", 78, new Rectangle(new Point(6, 394), new Size(188, 214)));         // cadre info lvl 1
            SpriteSheet.SetSpriteSheet("_Main_option", 79, new Rectangle(new Point(195, 394), new Size(188, 214)));         // cadre info lvl 2
            SpriteSheet.SetSpriteSheet("_Main_option", 80, new Rectangle(new Point(385, 394), new Size(188, 214)));         // cadre info lvl 3
            SpriteSheet.SetSpriteSheet("_Main_option", 81, new Rectangle(new Point(574, 394), new Size(188, 214)));         // cadre info lvl 4
            SpriteSheet.SetSpriteSheet("_Main_option", 82, new Rectangle(new Point(763, 394), new Size(188, 214)));         // cadre info lvl 5
            SpriteSheet.SetSpriteSheet("_Main_option", 83, new Rectangle(new Point(79, 268), new Size(15, 13)));         // pas de ligne de vue
            SpriteSheet.SetSpriteSheet("_Main_option", 84, new Rectangle(new Point(102, 269), new Size(22, 14)));         // PE modifable
            SpriteSheet.SetSpriteSheet("_Main_option", 85, new Rectangle(new Point(102, 283), new Size(22, 15)));         // PE non modifable
            SpriteSheet.SetSpriteSheet("_Main_option", 86, new Rectangle(new Point(79, 287), new Size(16, 11)));         // Ligne de vue
            #endregion ////////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////// gfx\general\classe\pays.dat
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.konoha, 0, new Rectangle(new Point(0, 0), new Size(42, 42)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.iwa, 0, new Rectangle(new Point(43, 0), new Size(42, 42)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.kiri, 0, new Rectangle(new Point(86, 0), new Size(42, 42)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.kumo, 0, new Rectangle(new Point(129, 0), new Size(42, 42)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.suna, 0, new Rectangle(new Point(172, 0), new Size(42, 42)));
            /////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////// gfx\general\classe\pays_thumbs.dat
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.konoha + "_thumbs", 0, new Rectangle(new Point(0, 0), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.iwa + "_thumbs", 0, new Rectangle(new Point(21, 0), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.kiri + "_thumbs", 0, new Rectangle(new Point(42, 0), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.kumo + "_thumbs", 0, new Rectangle(new Point(63, 0), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("pays_" + Enums.HiddenVillage.Names.suna + "_thumbs", 0, new Rectangle(new Point(84, 0), new Size(20, 20)));
            /////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////// gfx\general\obj\3\challenge.png
            SpriteSheet.SetSpriteSheet(Enums.BattleType.Type.FreeChallenge.ToString(), 0, new Rectangle(new Point(0, 0), new Size(40, 34)));
            SpriteSheet.SetSpriteSheet(Enums.BattleType.Type.FreeChallenge.ToString(), 1, new Rectangle(new Point(43, 0), new Size(44, 38)));
            SpriteSheet.SetSpriteSheet(Enums.BattleType.Type.FreeChallenge.ToString(), 2, new Rectangle(new Point(91, 0), new Size(44, 38)));

            /////////////////////// gfx\general\obj\1\all1.dat partie sorts
            SpriteSheet.SetSpriteSheet("__spellTarget", 0, new Rectangle(new Point(233, 232), new Size(30, 30)));
            SpriteSheet.SetSpriteSheet("0_spell", 0, new Rectangle(new Point(0, 0), new Size(31, 31)));     // rasengan
            SpriteSheet.SetSpriteSheet("1_spell", 0, new Rectangle(new Point(32, 0), new Size(31, 31)));     // shuriken
            SpriteSheet.SetSpriteSheet("2_spell", 0, new Rectangle(new Point(65, 0), new Size(31, 31)));     // rasen shuriken
            SpriteSheet.SetSpriteSheet("3_spell", 0, new Rectangle(new Point(97, 0), new Size(31, 31)));    // kage bunshin no jutsu
            // 4_spell est un sort pour l'invocation qui n'est pas utulisé par un joueur
            SpriteSheet.SetSpriteSheet("5_spell", 0, new Rectangle(new Point(129, 0), new Size(31, 31)));     // gamabunta
            SpriteSheet.SetSpriteSheet("6_spell", 0, new Rectangle(new Point(161, 0), new Size(31, 31)));     // transfert de vie
            SpriteSheet.SetSpriteSheet("7_spell", 0, new Rectangle(new Point(194, 0), new Size(31, 31)));     // transfert de pc
            SpriteSheet.SetSpriteSheet("8_spell", 0, new Rectangle(new Point(227, 0), new Size(31, 31)));     // transfert de pm
            SpriteSheet.SetSpriteSheet("9_spell", 0, new Rectangle(new Point(260, 0), new Size(31, 31)));     // transfert de puissance
            SpriteSheet.SetSpriteSheet("10_spell", 0, new Rectangle(new Point(0, 32), new Size(31, 31)));     // Etat Sennin
            SpriteSheet.SetSpriteSheet("11_spell", 0, new Rectangle(new Point(32, 32), new Size(31, 31)));     // katas des crapauds
            //... suites des sort de la classe naruto

            ///////////////////////////// gfx\general\obj\1\kabuto_spells.dat
            SpriteSheet.SetSpriteSheet("kabuto_edo", 0, new Rectangle(new Point(1, 1), new Size(32, 32)));

            //////////////////////////// gfx\general\classes\attackSprite0.dat
            SpriteSheet.SetSpriteSheet("naruto_attackSprite0", 0, new Rectangle(new Point(0, 2), new Size(25, 53)));
            SpriteSheet.SetSpriteSheet("naruto_attackSprite0", 1, new Rectangle(new Point(29, 0), new Size(27, 56)));
            SpriteSheet.SetSpriteSheet("naruto_attackSprite0", 2, new Rectangle(new Point(61, 0), new Size(27, 56)));
            SpriteSheet.SetSpriteSheet("naruto_attackSprite0", 3, new Rectangle(new Point(94, 2), new Size(24, 53)));

            //////////////////////////// gfx\general\classes\attackSprite0.dat
            SpriteSheet.SetSpriteSheet("iruka_attackSprite0", 0, new Rectangle(new Point(0, 0), new Size(25, 62)));
            SpriteSheet.SetSpriteSheet("iruka_attackSprite0", 1, new Rectangle(new Point(31, 0), new Size(29, 63)));
            SpriteSheet.SetSpriteSheet("iruka_attackSprite0", 2, new Rectangle(new Point(65, 0), new Size(29, 63)));
            SpriteSheet.SetSpriteSheet("iruka_attackSprite0", 3, new Rectangle(new Point(102, 2), new Size(23, 61)));

            // sort rasengan SortID,Couleur 0,Level
            SpriteSheet.SetSpriteSheet("naruto_spell00", 0, new Rectangle(new Point(0, 0), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("naruto_spell00", 1, new Rectangle(new Point(24, 0), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("naruto_spell00", 2, new Rectangle(new Point(53, 0), new Size(30, 31)));
            SpriteSheet.SetSpriteSheet("naruto_spell00", 3, new Rectangle(new Point(87, 0), new Size(35, 36)));
            SpriteSheet.SetSpriteSheet("naruto_spell00", 4, new Rectangle(new Point(126, 0), new Size(40, 41)));

            // sort rasengan SortID,Couleur 1,Level
            SpriteSheet.SetSpriteSheet("naruto_spell01", 0, new Rectangle(new Point(0, 44), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("naruto_spell01", 1, new Rectangle(new Point(24, 44), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("naruto_spell01", 2, new Rectangle(new Point(53, 44), new Size(30, 31)));
            SpriteSheet.SetSpriteSheet("naruto_spell01", 3, new Rectangle(new Point(87, 44), new Size(35, 36)));
            SpriteSheet.SetSpriteSheet("naruto_spell01", 4, new Rectangle(new Point(126, 44), new Size(40, 41)));

            // sort rasengan SortID,Couleur 2,Level
            SpriteSheet.SetSpriteSheet("naruto_spell02", 0, new Rectangle(new Point(0, 90), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("naruto_spell02", 1, new Rectangle(new Point(24, 90), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("naruto_spell02", 2, new Rectangle(new Point(53, 90), new Size(30, 31)));
            SpriteSheet.SetSpriteSheet("naruto_spell02", 3, new Rectangle(new Point(87, 90), new Size(35, 36)));
            SpriteSheet.SetSpriteSheet("naruto_spell02", 4, new Rectangle(new Point(126, 90), new Size(40, 41)));

            // sort rasengan SortID,Couleur 3,Level
            SpriteSheet.SetSpriteSheet("naruto_spell03", 0, new Rectangle(new Point(0, 136), new Size(20, 20)));
            SpriteSheet.SetSpriteSheet("naruto_spell03", 1, new Rectangle(new Point(24, 136), new Size(25, 25)));
            SpriteSheet.SetSpriteSheet("naruto_spell03", 2, new Rectangle(new Point(53, 136), new Size(30, 31)));
            SpriteSheet.SetSpriteSheet("naruto_spell03", 3, new Rectangle(new Point(87, 136), new Size(35, 36)));
            SpriteSheet.SetSpriteSheet("naruto_spell03", 4, new Rectangle(new Point(126, 136), new Size(40, 41)));

            // sort Shuriken
            SpriteSheet.SetSpriteSheet("shuriken", 0, new Rectangle(new Point(0, 0), new Size(26, 26)));
            SpriteSheet.SetSpriteSheet("shuriken", 1, new Rectangle(new Point(25, 0), new Size(26, 26)));
            SpriteSheet.SetSpriteSheet("shuriken", 2, new Rectangle(new Point(52, 0), new Size(26, 26)));
            SpriteSheet.SetSpriteSheet("shuriken", 3, new Rectangle(new Point(75, 0), new Size(26, 26)));
            SpriteSheet.SetSpriteSheet("shuriken", 4, new Rectangle(new Point(99, 0), new Size(26, 26)));
            SpriteSheet.SetSpriteSheet("shuriken", 5, new Rectangle(new Point(124, 0), new Size(26, 26)));

            #region sort rasen shuriken
            // sprite1
            // lvl 1
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L1", 0, new Rectangle(new Point(0, 8), new Size(43, 31)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L1", 0, new Rectangle(new Point(0, 101), new Size(43, 31)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L1", 0, new Rectangle(new Point(0, 191), new Size(43, 31)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L1", 0, new Rectangle(new Point(0, 278), new Size(43, 31)));
            // lvl 2
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L2", 0, new Rectangle(new Point(51, 8), new Size(52, 37)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L2", 0, new Rectangle(new Point(53, 103), new Size(52, 37)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L2", 0, new Rectangle(new Point(51, 191), new Size(52, 37)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L2", 0, new Rectangle(new Point(51, 278), new Size(52, 37)));
            // lvl 3
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L3", 0, new Rectangle(new Point(110, 8), new Size(60, 43)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L3", 0, new Rectangle(new Point(111, 103), new Size(60, 43)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L3", 0, new Rectangle(new Point(110, 188), new Size(60, 43)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L3", 0, new Rectangle(new Point(111, 278), new Size(60, 43)));
            // lvl 4
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L4", 0, new Rectangle(new Point(183, 8), new Size(69, 50)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L4", 0, new Rectangle(new Point(180, 104), new Size(69, 50)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L4", 0, new Rectangle(new Point(180, 188), new Size(69, 50)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L4", 0, new Rectangle(new Point(180, 278), new Size(69, 50)));
            // lvl 5
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L5", 0, new Rectangle(new Point(264, 8), new Size(86, 62)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L5", 0, new Rectangle(new Point(260, 93), new Size(86, 62)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L5", 0, new Rectangle(new Point(260, 188), new Size(86, 62)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L5", 0, new Rectangle(new Point(260, 273), new Size(86, 62)));
            // sprite 2
            // lvl 1
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L1", 1, new Rectangle(new Point(0, 3), new Size(44, 42)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L1", 1, new Rectangle(new Point(0, 96), new Size(44, 42)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L1", 1, new Rectangle(new Point(0, 186), new Size(44, 42)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L1", 1, new Rectangle(new Point(0, 273), new Size(44, 42)));
            // lvl 2
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L2", 1, new Rectangle(new Point(51, 2), new Size(53, 51)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L2", 1, new Rectangle(new Point(53, 97), new Size(53, 51)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L2", 1, new Rectangle(new Point(51, 185), new Size(53, 51)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L2", 1, new Rectangle(new Point(51, 272), new Size(53, 51)));
            // lvl 3
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L3", 1, new Rectangle(new Point(111, 1), new Size(60, 58)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L3", 1, new Rectangle(new Point(112, 96), new Size(60, 58)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L3", 1, new Rectangle(new Point(111, 181), new Size(60, 58)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L3", 1, new Rectangle(new Point(112, 271), new Size(60, 58)));
            // lvl 4
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L4", 1, new Rectangle(new Point(183, 1), new Size(70, 66)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L4", 1, new Rectangle(new Point(180, 97), new Size(70, 66)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L4", 1, new Rectangle(new Point(180, 181), new Size(70, 66)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L4", 1, new Rectangle(new Point(180, 271), new Size(70, 66)));
            // lvl 5
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L5", 1, new Rectangle(new Point(265, 0), new Size(85, 81)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L5", 1, new Rectangle(new Point(261, 84), new Size(85, 82)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L5", 1, new Rectangle(new Point(261, 179), new Size(85, 82)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L5", 1, new Rectangle(new Point(261, 264), new Size(85, 82)));
            // sprite 3
            // lvl 1
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L1", 2, new Rectangle(new Point(1, 0), new Size(42, 48)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L1", 2, new Rectangle(new Point(1, 91), new Size(42, 50)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L1", 2, new Rectangle(new Point(1, 181), new Size(42, 50)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L1", 2, new Rectangle(new Point(1, 268), new Size(42, 50)));
            // lvl 2
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L2", 2, new Rectangle(new Point(52, 0), new Size(51, 57)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L2", 2, new Rectangle(new Point(54, 92), new Size(51, 60)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L2", 2, new Rectangle(new Point(52, 180), new Size(51, 60)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L2", 2, new Rectangle(new Point(52, 267), new Size(51, 60)));
            // lvl 3
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L3", 2, new Rectangle(new Point(111, 0), new Size(59, 54)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L3", 2, new Rectangle(new Point(112, 90), new Size(59, 69)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L3", 2, new Rectangle(new Point(111, 175), new Size(59, 69)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L3", 2, new Rectangle(new Point(112, 265), new Size(59, 69)));
            // lvl 4
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L4", 2, new Rectangle(new Point(184, 0), new Size(68, 73)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L4", 2, new Rectangle(new Point(181, 90), new Size(68, 79)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L4", 2, new Rectangle(new Point(181, 174), new Size(68, 79)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L4", 2, new Rectangle(new Point(181, 264), new Size(68, 79)));
            // lvl 5
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L5", 2, new Rectangle(new Point(261, 0), new Size(89, 79)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L5", 2, new Rectangle(new Point(261, 80), new Size(89, 93)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L5", 2, new Rectangle(new Point(261, 175), new Size(89, 83)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L5", 2, new Rectangle(new Point(261, 260), new Size(89, 90)));
            // sprite 4
            // lvl 1
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L1", 3, new Rectangle(new Point(5, 0), new Size(33, 46)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L1", 3, new Rectangle(new Point(5, 93), new Size(33, 46)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L1", 3, new Rectangle(new Point(5, 183), new Size(33, 46)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L1", 3, new Rectangle(new Point(5, 270), new Size(33, 46)));
            // lvl 2
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L2", 3, new Rectangle(new Point(58, 0), new Size(39, 54)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L2", 3, new Rectangle(new Point(60, 94), new Size(39, 55)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L2", 3, new Rectangle(new Point(58, 182), new Size(39, 55)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L2", 3, new Rectangle(new Point(58, 269), new Size(39, 55)));
            // lvl 3
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L3", 3, new Rectangle(new Point(118, 0), new Size(45, 61)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L3", 3, new Rectangle(new Point(119, 93), new Size(45, 63)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L3", 3, new Rectangle(new Point(118, 178), new Size(45, 63)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L3", 3, new Rectangle(new Point(119, 268), new Size(45, 63)));
            // lvl 4
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L4", 3, new Rectangle(new Point(193, 0), new Size(52, 68)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L4", 3, new Rectangle(new Point(190, 93), new Size(52, 71)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L4", 3, new Rectangle(new Point(190, 177), new Size(52, 71)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L4", 3, new Rectangle(new Point(190, 267), new Size(52, 71)));
            // lvl 5
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C0_L5", 3, new Rectangle(new Point(276, 0), new Size(64, 83)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C1_L5", 3, new Rectangle(new Point(272, 79), new Size(64, 89)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C2_L5", 3, new Rectangle(new Point(272, 174), new Size(64, 89)));
            SpriteSheet.SetSpriteSheet("rasen_shuriken_C3_L5", 3, new Rectangle(new Point(272, 259), new Size(64, 89)));
            #endregion

            // image des combat
            SpriteSheet.SetSpriteSheet("vs", 0, new Rectangle(new Point(264, 229), new Size(34, 43)));
        }
    }
}
