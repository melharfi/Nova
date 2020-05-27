namespace SERVER
{
    public static class isFreeCellToWalk
	{
		public static bool Start (Point p)
        {
            // contient tous les tuiles non accessible sur la map
            // partie obstacles du map
            if (p.X >= 540 && p.X < 570 && p.Y >= 90 && p.Y < 120)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 180 && p.X < 210 && p.Y >= 330 && p.Y < 360)    // obstacles 1 posés sur le map
                return false;
            else if (p.X >= 330 && p.X < 360 && p.Y >= 120 && p.Y < 150)    // obstacles 2 posés sur le map
                return false;
            // ligne 1
            else if (p.X >= 0 && p.X < 30 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 2
            else if (p.X >= 30 && p.X < 60 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 3
            else if (p.X >= 60 && p.X < 90 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 4
            else if (p.X >= 90 && p.X < 120 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            // ligne 5
            else if (p.X >= 960 && p.X < 990 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 6
            else if (p.X >= 930 && p.X < 960 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 7
            else if (p.X >= 900 && p.X < 930 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 8
            else if (p.X >= 870 && p.X < 900 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            else if (p.X > ScreenManager.TileWidth * 30 || p.X < 0 || p.Y > ScreenManager.TileHeight * 30 || p.Y < 0)
                return false;
            else
                return true;
        }
        public static bool _0_0_0(Point p)
        {
            if (p.X >= 420 && p.X < 450 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 420 && p.X < 450 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 330 && p.Y < 360)
                return false;
            else
                return true;
        }
    }

	public static class isFreeCellToSpell
    {
        // un probleme pas encore reglé, quand il ya un lac qui est un obstacle pour le waypoint et pour le sort mais qui ne devrais pas bloquer la LDV du sort
        // il faut ajouter un mechanisme qui fait que la tuile est un obstacle mais avec une montion qui bloque ou pas la LDV, donc ajouter un autre controle genre isFreeCellToSpell(new Point(40,50), true) et la fonction dois etre du genre Start(Point p, Bool BlockLDV), et apres associer a chaque condition qui correspond a une tuile une donnée 
        //if (p.X >= 540 && p.X< 570 && p.Y >= 90 && p.Y< 120)
        //{
        //      if(BlockLDV)
        //          return false;
        //      else
        //          return true;


        public static bool Start(Point p)
		{
            // contient tous les tuiles non accessible sur la map pour le mode marche
            if (p.X >= 540 && p.X < 570 && p.Y >= 90 && p.Y < 120)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 180 && p.X < 210 && p.Y >= 330 && p.Y < 360)    // obstacles 1 posés sur le map
                return false;
            else if (p.X >= 330 && p.X < 360 && p.Y >= 120 && p.Y < 150)    // obstacles 2 posés sur le map
                return false;
            // ligne 1
            else if (p.X >= 0 && p.X < 30 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 2
            else if (p.X >= 30 && p.X < 60 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 3
            else if (p.X >= 60 && p.X < 90 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 4
            else if (p.X >= 90 && p.X < 120 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            // ligne 5
            else if (p.X >= 960 && p.X < 990 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 6
            else if (p.X >= 930 && p.X < 960 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 7
            else if (p.X >= 900 && p.X < 930 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 8
            else if (p.X >= 870 && p.X < 900 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            else if (p.X > ScreenManager.TileWidth * 30 || p.X < 0 || p.Y > ScreenManager.TileHeight * 30 || p.Y < 0)
                return false;
            else
                return true;
		}
        public static bool _0_0_0(Point p)
        {
            if (p.X >= 420 && p.X < 450 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 420 && p.X < 450 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 330 && p.Y < 360)
                return false;
            else
                return true;
        }
	}
}