namespace Basket{
    public enum ShotType { Straight, FadeAway, Layup, CatchShot, Bank, BankBack, Pump, Hook, Set, FreeThrow };
    public enum DunkType { Simple, Reverse, TweenLeg, BehindBack, Windmill, Axe, Cuttle };
    public enum DribbleType { CrossOver, BehindBack, ShameGod, Rotate, StepBack, FakePass };
    public enum PassType { Standard, Bullet, High, Bounce, AllyOop, BehindBack, Jerk, Quick };
    public enum RangeType { RimRange, MidRange, LongRange };
    public enum BasketGameState { JumpBall, T1Ball, T2Ball, LooseBall, OutBounds, FT, TimeOut, OverTime, Over };
    public enum BasketPos { PG = 1, SG = 2, SF = 3, PF = 4, C = 5};
}
