using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "a skeletal corpse" )]
	public class Skeleton : BaseCreature
	{
		[Constructable]
		public Skeleton() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeleton";
			Body = Utility.RandomList( 50, 56 );
			BaseSoundID = 0x48D;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 16, 40 );

			SetHits( 34, 48 );

			SetDamage( 3, 7 );

			SetSkill( SkillName.MagicResist, 45.1, 60.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Wrestling, 45.1, 55.0 );

			Fame = 450;
			Karma = -450;

			VirtualArmor = 16;

			switch ( Utility.Random( 5 ))
			{
				case 0: PackItem( new BoneArms() ); break;
				case 1: PackItem( new BoneChest() ); break;
				case 2: PackItem( new BoneGloves() ); break;
				case 3: PackItem( new BoneLegs() ); break;
				case 4: PackItem( new BoneHelm() ); break;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public Skeleton( Serial serial ) : base( serial )
		{
		}

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
