delete from m_tutorial_flow;
REPLACE INTO `m_tutorial_flow` (`tutorial_id`,`scene_name`,`next_tutorial_id`,`kind`) VALUES 
 ('40','Download','1','SelectCharacter'),
 ('1','Tutorial','2','PrologueADV'),
 ('2','Tutorial','3','SelectTypeADV'),
 ('3','Tutorial','20','SelectType'),
 ('31','SongSelect','32','LiveSongSelect'),
 ('32','SongSelect','33','LiveGuestSelect'),
 ('33','SongSelect','34','LiveUnitSelect'),
 ('34','SongSelect','35','LivePlay'),
 ('35','Tutorial','22','LiveResult'),
 ('20','Gacha','21','Gacha'),
 ('21','Gacha','4','GachaResult'),
 ('4','UnitEdit','5','UnitEdit'),
 ('5','','31','SongSelect'),
 ('22','Download','23','Questionnaire'),
 ('23','MyPage','0','BegginerMission'),
 ('0','MyPage','0','TutorialEnd');
