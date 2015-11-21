using System;
using Server.Items;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	public class Stealth
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Stealth].Callback = new SkillUseCallback( OnUse );
		}

		public static double HidingRequirement { get { return 80.0; } }

		public static int[,] ArmorTable { get { return m_ArmorTable; } }
		private static int[,] m_ArmorTable = new int[,]
			{
							//	Gorget	Gloves	Helmet	Arms	Legs	Chest	Shield
				/* Cloth	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Leather	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Studded	*/	{ 2,	2,		0,		4,		6,		10,		0 },
				/* Bone		*/ 	{ 0,	5,		10,		10,		15,		25,		0 },
				/* Spined	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Horned	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Barbed	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Ring		*/	{ 0,	5,		0,		10,		15,		25,		0 },
				/* Chain	*/	{ 0,	0,		10,		0,		15,		25,		0 },
				/* Plate	*/	{ 5,	5,		10,		10,		15,		25,		0 },
				/* Dragon	*/	{ 0,	5,		10,		10,		15,		25,		0 }
			};

		public static int GetArmorRating( Mobile m )
		{
			return (int)m.ArmorRating;
		}

		public static TimeSpan OnUse( Mobile m )
		{
			if ( !m.Hidden )
			{
				m.SendLocalizedMessage( 502725 ); // You must hide first
			}
			else if ( m.Skills[SkillName.Hiding].Base < HidingRequirement )
			{
				m.SendLocalizedMessage( 502726 ); // You are not hidden well enough.  Become better at hiding.
				m.RevealingAction();
			}
			else if( !m.CanBeginAction( typeof( Stealth ) ) )
			{
				m.SendLocalizedMessage( 1063086 ); // You cannot use this skill right now.
				m.RevealingAction();
			}
			else
			{
				int armorRating = GetArmorRating( m );

				if( armorRating >= 26 )
				{
					m.SendLocalizedMessage( 502727 ); // You could not hope to move quietly wearing this much armor.
					m.RevealingAction();
				}
				else if( m.CheckSkill( SkillName.Stealth, -20.0 + (armorRating * 2), 80.0 + (armorRating * 2) ) )
				{
					int steps = (int)(m.Skills[SkillName.Stealth].Value / 10.0);

					if( steps < 1 )
						steps = 1;

					m.AllowedStealthSteps = steps;

					PlayerMobile pm = m as PlayerMobile; // IsStealthing should be moved to Server.Mobiles

					if( pm != null )
						pm.IsStealthing = true;

					m.SendLocalizedMessage( 502730 ); // You begin to move quietly.

					return TimeSpan.FromSeconds( 10.0 );
				}
				else
				{
					m.SendLocalizedMessage( 502731 ); // You fail in your attempt to move unnoticed.
					m.RevealingAction();
				}
			}

			return TimeSpan.FromSeconds( 10.0 );
		}
	}
}