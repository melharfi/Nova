namespace SERVER.Enums
{
    public class Rang
    {
        public enum official
        {
            neutral = 0,
            student = 1,
            genin = 2,
            chuunin = 3,
            juunin = 4,
            kage = 5
        }

        public enum special
        {
            neutral = 0,
            anbu = 1,           //C'est une division de juunins créée pour servir de garde personnelle du Hokage. Ce groupe s'occupe des missions les plus perilleuses : assasinats, escortes, etc... C'est l'élite des ninjas. Kakashi a lui-même été par le passé un membre d'Anbu. Pour des raisons de sécurité, les anbus gardent l'anonymat, et portent des masques
            nukenin = 2,        //[déserteurs)] Ninjas qui ont abandonné leur village. En conséquence, ils sont traqués par les chasseurs de déserteurs de leur village. Ils deviennent souvent mercenaires pour des crapules (Gato par exemple). Zabuza et Haku sont des déserteurs, ainsi que Itachi. 
            oinins = 3,        //[chasseurs de déserteurs] Anbus qui traquent les déserteurs pour protéger les secrets de leur village. Ils font disparaître les corps des déserteurs. Haku s'est fait passer pour un chasseur de déserteurs pour sauver Zabuza, que Kakashi s'appretait à achever. 
            specialJuunin = 4   //Un juunin qui se spécialise dans un domaine spécifique. Morino Ibiki, par exemple, est chef de l'unité d'interrogation et de torture de Konoha. Anko est une juunin spécial en raison de sa connaissance sur l'émergeant et dangereux village du son. Les anbus sont également des juunins spéciaux. 
        }
    }
}
