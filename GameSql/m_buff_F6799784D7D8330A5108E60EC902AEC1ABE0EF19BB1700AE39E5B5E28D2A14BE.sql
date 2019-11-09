delete from m_buff;
REPLACE INTO `m_buff` (`buff_id`,`name`,`description`,`effect_type`,`effect_kind`,`effect`,`logo_path`,`icon_path`) VALUES 
 ('1','P確率3倍','Step5のG以上1枚確定カードのP確率がアップします','1','9','3','buff_logo_p3.png.enc','buff_icon_p3.png.enc'),
 ('2','G+確率3倍','Step5のG以上1枚確定カードのG+確率がアップします','1','7','3','buff_logo_gp3.png.enc','buff_icon_gp3.png.enc'),
 ('3','おまけ（P確定スターチケット）','Step5でP確定スターチケット1枚をGETできます','3','0','1','buff_logo_pticket.png.enc','buff_icon_pticket.png.enc'),
 ('4','おまけ（G確定スターチケット）','Step5でG確定スターチケット1枚をGETできます','3','0','2','buff_logo_gticket.png.enc','buff_icon_gticket.png.enc'),
 ('5','おまけ（ガチャ開放チケット（シサラ））','Step5でガチャ開放チケット（シサラ）1枚をGETできます','3','0','3','buff_logo_limit_1.png.enc','buff_icon_limit_1.png.enc'),
 ('6','おまけ（ガチャ開放チケット（ミワコ））','Step5でガチャ開放チケット（ミワコ）1枚をGETできます','3','0','4','buff_logo_limit_42.png.enc','buff_icon_limit_42.png.enc'),
 ('7','おまけ（ガチャ開放チケット（ミミ））','Step5でガチャ開放チケット（ミミ）1枚をGETできます','3','0','5','buff_logo_limit_8.png.enc','buff_icon_limit_8.png.enc'),
 ('8','おまけ（ガチャ開放チケット（チャチャ））','Step5でガチャ開放チケット（チャチャ）1枚をGETできます','3','0','6','buff_logo_limit_40.png.enc','buff_icon_limit_40.png.enc'),
 ('9','おまけ（オーディションチケット）','Step5でオーディションチケット1枚をGETできます','3','0','7','buff_logo_audticket.png.enc','buff_icon_audticket.png.enc'),
 ('10','おまけ（セブンスポイント100pt）','Step5でセブンスポイント100ptをGETできます','3','0','8','buff_logo_point100.png.enc','buff_icon_point100.png.enc'),
 ('11','おまけ（ドリームメダル10枚）','Step5でドリームメダル10枚をGETできます','3','0','9','buff_logo_dream.png.enc','buff_icon_dream.png.enc');
