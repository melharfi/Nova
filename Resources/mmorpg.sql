/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : mmorpg

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2020-05-06 18:37:02
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `bannedip`
-- ----------------------------
DROP TABLE IF EXISTS `bannedip`;
CREATE TABLE `bannedip` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `ip` varchar(15) NOT NULL,
  `reson` varchar(30) NOT NULL,
  `censure` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of bannedip
-- ----------------------------

-- ----------------------------
-- Table structure for `banneduser`
-- ----------------------------
DROP TABLE IF EXISTS `banneduser`;
CREATE TABLE `banneduser` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `user` varchar(25) NOT NULL,
  `reson` varchar(200) NOT NULL,
  `temps` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_3` (`id`),
  KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of banneduser
-- ----------------------------

-- ----------------------------
-- Table structure for `classes`
-- ----------------------------
DROP TABLE IF EXISTS `classes`;
CREATE TABLE `classes` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `classeId` int(1) NOT NULL,
  `classeName` varchar(15) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of classes
-- ----------------------------
INSERT INTO `classes` VALUES ('1', '1', 'naruto');
INSERT INTO `classes` VALUES ('2', '2', 'choji');
INSERT INTO `classes` VALUES ('3', '3', 'kabuto');
INSERT INTO `classes` VALUES ('4', '4', 'ino');
INSERT INTO `classes` VALUES ('5', '5', 'iruka');
INSERT INTO `classes` VALUES ('6', '6', 'kanku');
INSERT INTO `classes` VALUES ('7', '7', 'shikamaru');
INSERT INTO `classes` VALUES ('8', '8', 'sakura');

-- ----------------------------
-- Table structure for `connected`
-- ----------------------------
DROP TABLE IF EXISTS `connected`;
CREATE TABLE `connected` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `ip` varchar(15) NOT NULL,
  `user` varchar(16) NOT NULL,
  `pseudo` varchar(20) NOT NULL,
  `uid` varchar(45) NOT NULL,
  `date` varchar(16) NOT NULL,
  `timestamp` int(20) NOT NULL,
  `map` varchar(15) NOT NULL,
  `map_position` varchar(15) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of connected
-- ----------------------------

-- ----------------------------
-- Table structure for `effects`
-- ----------------------------
DROP TABLE IF EXISTS `effects`;
CREATE TABLE `effects` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `effectID` int(11) NOT NULL,
  `type` varchar(15) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of effects
-- ----------------------------

-- ----------------------------
-- Table structure for `logcounter`
-- ----------------------------
DROP TABLE IF EXISTS `logcounter`;
CREATE TABLE `logcounter` (
  `id` int(50) NOT NULL AUTO_INCREMENT,
  `user` varchar(20) NOT NULL,
  `timestamp` int(15) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of logcounter
-- ----------------------------

-- ----------------------------
-- Table structure for `logerror`
-- ----------------------------
DROP TABLE IF EXISTS `logerror`;
CREATE TABLE `logerror` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `error` varchar(100) NOT NULL,
  `date` varchar(25) NOT NULL,
  `user` varchar(15) NOT NULL,
  `ip` varchar(15) NOT NULL,
  `detail` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of logerror
-- ----------------------------

-- ----------------------------
-- Table structure for `mapobj`
-- ----------------------------
DROP TABLE IF EXISTS `mapobj`;
CREATE TABLE `mapobj` (
  `id` int(250) NOT NULL AUTO_INCREMENT,
  `map` varchar(20) NOT NULL,
  `map_position` varchar(10) NOT NULL,
  `state` varchar(10) NOT NULL DEFAULT 'dynamic' COMMENT 'static = objet fix / dynamic = objet temporaire',
  `obj` varchar(100) NOT NULL,
  `assoc` varchar(50) NOT NULL COMMENT 'contiens des infos comme la team ou l''id du commbat',
  `idBattle` int(10) NOT NULL COMMENT 'contiens l''id du combat s''il sagit d''un objet de combat',
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of mapobj
-- ----------------------------

-- ----------------------------
-- Table structure for `npc`
-- ----------------------------
DROP TABLE IF EXISTS `npc`;
CREATE TABLE `npc` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `npcId` int(11) NOT NULL,
  `npcName` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of npc
-- ----------------------------
INSERT INTO `npc` VALUES ('1', '0', 'Iruka');

-- ----------------------------
-- Table structure for `npcaction`
-- ----------------------------
DROP TABLE IF EXISTS `npcaction`;
CREATE TABLE `npcaction` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `npcID` int(11) DEFAULT NULL COMMENT 'facultative',
  `action` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of npcaction
-- ----------------------------
INSERT INTO `npcaction` VALUES ('1', '0', '0');

-- ----------------------------
-- Table structure for `npcspawn`
-- ----------------------------
DROP TABLE IF EXISTS `npcspawn`;
CREATE TABLE `npcspawn` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `npcID` int(11) NOT NULL,
  `npcName` varchar(255) NOT NULL,
  `map` varchar(255) NOT NULL,
  `posX` int(11) NOT NULL,
  `posY` int(11) NOT NULL,
  `typeDialog` varchar(255) NOT NULL,
  `comment` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of npcspawn
-- ----------------------------
INSERT INTO `npcspawn` VALUES ('1', '0', 'Iruka', 'Start', '20', '20', 'Speek', null);

-- ----------------------------
-- Table structure for `paramsbkp`
-- ----------------------------
DROP TABLE IF EXISTS `paramsbkp`;
CREATE TABLE `paramsbkp` (
  `id` int(1) NOT NULL AUTO_INCREMENT,
  `Port` int(6) NOT NULL DEFAULT '7070' COMMENT 'Port d''ecoute',
  `MaxConnexions` int(2) NOT NULL DEFAULT '-1' COMMENT 'maximum de connexion s simultannés, -1= illimité',
  `MaxTimeAfk` int(5) NOT NULL DEFAULT '900' COMMENT 'temps maximum avant de kicker un utilisateur de l''objet server.Client representé en seconds,900=15min',
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of paramsbkp
-- ----------------------------
INSERT INTO `paramsbkp` VALUES ('1', '7070', '500', '900');

-- ----------------------------
-- Table structure for `players`
-- ----------------------------
DROP TABLE IF EXISTS `players`;
CREATE TABLE `players` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `pseudo` varchar(10) NOT NULL,
  `user` varchar(25) NOT NULL,
  `spirit` varchar(15) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL DEFAULT 'neutre',
  `spiritLevel` int(2) NOT NULL DEFAULT '1',
  `pvpEnabled` tinyint(1) NOT NULL DEFAULT '0',
  `classe` varchar(15) NOT NULL,
  `map` varchar(15) NOT NULL DEFAULT 'Start',
  `map_position` varchar(15) NOT NULL DEFAULT '15/9',
  `directionLook` tinyint(1) NOT NULL DEFAULT '0',
  `size` varchar(7) NOT NULL,
  `level` int(3) NOT NULL DEFAULT '1',
  `sexe` varchar(1) NOT NULL,
  `hiddenVillage` varchar(10) NOT NULL,
  `maskColorString` varchar(44) NOT NULL,
  `maxHealth` int(10) NOT NULL DEFAULT '0' COMMENT 'points de vie',
  `currentHealth` int(10) NOT NULL DEFAULT '0',
  `inBattle` int(1) NOT NULL DEFAULT '0',
  `inBattleType` varchar(15) NOT NULL,
  `inBattleID` int(15) NOT NULL,
  `officialRang` int(1) NOT NULL DEFAULT '0',
  `xp` int(15) NOT NULL DEFAULT '0' COMMENT 'xp du niveau avant le up',
  `doton` int(10) NOT NULL DEFAULT '0',
  `katon` int(10) NOT NULL DEFAULT '0',
  `futon` int(10) NOT NULL DEFAULT '0',
  `raiton` int(10) NOT NULL DEFAULT '0',
  `suiton` int(10) NOT NULL DEFAULT '0',
  `puissance` int(10) NOT NULL DEFAULT '0',
  `usingDoton` int(10) NOT NULL DEFAULT '0',
  `usingKaton` int(10) NOT NULL DEFAULT '0',
  `usingFuton` int(10) NOT NULL DEFAULT '0',
  `usingRaiton` int(10) NOT NULL DEFAULT '0',
  `usingSuiton` int(10) NOT NULL DEFAULT '0',
  `equipedDoton` int(6) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `equipedKaton` int(6) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `equipedFuton` int(6) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `equipedRaiton` int(6) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `equipedSuiton` int(6) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `equipedPwer` int(10) NOT NULL DEFAULT '0' COMMENT 'puissance dans les items utilisés',
  `pc` int(2) NOT NULL DEFAULT '5' COMMENT 'point de chakra',
  `pm` int(5) NOT NULL DEFAULT '3',
  `pe` int(5) NOT NULL DEFAULT '0',
  `cd` int(5) NOT NULL DEFAULT '0' COMMENT 'coup dangeureu cd',
  `summons` int(6) NOT NULL DEFAULT '1',
  `initiative` int(6) NOT NULL DEFAULT '100',
  `job1` varchar(15) NOT NULL,
  `job2` varchar(15) NOT NULL,
  `specialite1` varchar(15) NOT NULL,
  `specialite2` varchar(15) NOT NULL,
  `maxWeight` int(10) NOT NULL DEFAULT '100',
  `currentWeight` int(6) NOT NULL DEFAULT '0',
  `ryo` int(20) NOT NULL DEFAULT '0',
  `resiDotonPercent` int(3) NOT NULL DEFAULT '0',
  `resiKatonPercent` int(3) NOT NULL DEFAULT '0',
  `resiFutonPercent` int(3) NOT NULL DEFAULT '0',
  `resiRaitonPercent` int(3) NOT NULL DEFAULT '0',
  `resiSuitonPercent` int(3) NOT NULL DEFAULT '0',
  `resiDotonFix` int(10) NOT NULL DEFAULT '0',
  `resiKatonFix` int(10) NOT NULL DEFAULT '0',
  `resiFutonFix` int(10) NOT NULL DEFAULT '0',
  `resiRaitonFix` int(10) NOT NULL DEFAULT '0',
  `resiSuitonFix` int(10) NOT NULL DEFAULT '0',
  `resiFix` int(5) NOT NULL DEFAULT '0',
  `dodgePC` int(3) NOT NULL DEFAULT '0',
  `dodgePM` int(3) NOT NULL DEFAULT '0',
  `dodgePE` int(3) NOT NULL DEFAULT '0',
  `dodgeCD` int(3) NOT NULL DEFAULT '0',
  `removePC` int(3) NOT NULL DEFAULT '0',
  `removePM` int(3) NOT NULL DEFAULT '0',
  `removePE` int(3) NOT NULL DEFAULT '0',
  `removeCD` int(3) NOT NULL DEFAULT '0',
  `escape` int(4) NOT NULL DEFAULT '0',
  `blocage` int(4) NOT NULL DEFAULT '0',
  `sorts` varchar(255) NOT NULL COMMENT 'sortID:emplacementID:level:couleurID|...',
  `domDotonFix` int(5) NOT NULL DEFAULT '0',
  `domKatonFix` int(5) NOT NULL DEFAULT '0',
  `domFutonFix` int(5) NOT NULL DEFAULT '0',
  `domRaitonFix` int(5) NOT NULL DEFAULT '0',
  `domSuitonFix` int(5) NOT NULL DEFAULT '0',
  `domFix` int(5) NOT NULL DEFAULT '0',
  `spellPointLeft` int(5) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`),
  KEY `id_3` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of players
-- ----------------------------
INSERT INTO `players` VALUES ('26', 'narutox', 'admin', 'angel', '15', '1', 'naruto', 'Start', '15/14', '3', '', '30', '', 'kumo', '52-153-201/253-197-134/12-46-88', '500', '500', '0', '', '0', '4', '1999', '100', '200', '300', '400', '500', '50', '600', '500', '3000', '100', '200', '151', '152', '153', '154', '155', '75', '12', '6', '2', '40', '1', '200', 'macon', 'tailleur', 'chasseur', 'artiste', '100', '10', '1301587', '5', '10', '20', '30', '30', '5', '6', '7', '8', '9', '10', '1', '2', '3', '4', '5', '6', '7', '8', '0', '100', '0:1:5:2/1:2:5:0/2:3:5:0/3:4:5:1/5:5:5:1/6:6:5:0/7:7:5:0/8:8:5:0/9:9:5:0/10:10:5:0/11:11:5:0', '15', '16', '17', '18', '19', '20', '0');
INSERT INTO `players` VALUES ('27', 'pop', 'admin2', 'demon', '5', '0', 'ino', 'Start', '13/9', '1', '', '1', '', 'konoha', 'null/null/null', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '100', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('30', 'mmm', 'admin3', 'neutral', '1', '0', 'naruto', 'Start', '18/5', '1', '', '1', '', 'konoha', '128-128-255/0-0-128/255-128-128', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '120', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0:1:3:0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('33', 'kabutoo', 'morpher', 'neutral', '1', '0', 'kabuto', 'Start', '19/8', '1', '', '1', '', 'kumo', '18-103-24/49-219-62/242-209-149', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '140', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('34', 'tmp1', 'admin4', 'neutral', '1', '0', 'naruto', 'Start', '19/10', '1', '', '1', '', 'konoha', 'null/null/null', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '130', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0:1:2:0/3:2:5:2', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('36', 'kabox', 'admin2', 'demon', '15', '1', 'naruto', 'Start', '25/10', '1', '', '1', '', 'konoha', '236-227-66/236-112-51/248-200-177', '1000', '1000', '0', '', '0', '0', '0', '0', '0', '300', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '10', '8', '3', '0', '1', '150', '', '', '', '', '100', '0', '0', '15', '25', '35', '45', '55', '0', '0', '0', '0', '0', '0', '11', '12', '13', '14', '15', '16', '17', '18', '0', '0', '0:1:5:0/3:2:5:3', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('81', 'player2', 'admin', 'neutral', '1', '0', 'choji', 'Start', '15/9', '0', '', '1', '', 'suna', '38-62-157/81-171-238/172-239-253', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '100', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('83', 'player3', 'admin', 'neutral', '1', '0', 'kabuto', 'Start', '15/9', '0', '', '1', '', 'suna', '225-214-66/228-141-63/151-103-4', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '100', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('85', 'player5', 'admin', 'neutral', '1', '0', 'naruto', 'Start', '15/9', '0', '', '1', '', 'kiri', 'null/null/null', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '100', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('86', '9846495464', 'admin2', 'neutral', '1', '0', 'ino', 'Start', '15/9', '0', '', '1', '', 'kumo', '0-0-255/0-0-128/64-0-128', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '100', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `players` VALUES ('88', 'lololololo', 'admin2', 'neutral', '1', '0', 'naruto', 'Start', '15/9', '0', '', '1', '', 'konoha', 'null/null/null', '60', '60', '0', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '3', '0', '0', '1', '100', '', '', '', '', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '0', '0', '0', '0', '0', '0', '0');

-- ----------------------------
-- Table structure for `quete`
-- ----------------------------
DROP TABLE IF EXISTS `quete`;
CREATE TABLE `quete` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nom_quete` varchar(25) NOT NULL,
  `totalSteps` int(10) NOT NULL,
  `currentStep` int(10) NOT NULL,
  `submited` tinyint(1) NOT NULL,
  `pseudo` varchar(0) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of quete
-- ----------------------------

-- ----------------------------
-- Table structure for `spells`
-- ----------------------------
DROP TABLE IF EXISTS `spells`;
CREATE TABLE `spells` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `spellID` int(11) NOT NULL,
  `spellName` varchar(50) NOT NULL,
  `level` int(1) NOT NULL,
  `pc` int(11) NOT NULL DEFAULT '0',
  `pe` int(11) NOT NULL DEFAULT '0',
  `technique` varchar(20) NOT NULL,
  `rang` varchar(1) NOT NULL,
  `peModifiable` tinyint(1) NOT NULL,
  `ligneDeVue` tinyint(1) NOT NULL,
  `minValue` int(10) NOT NULL,
  `maxValue` int(10) NOT NULL,
  `cd` int(11) NOT NULL,
  `relanceInterval` int(11) NOT NULL,
  `relanceParJoueur` int(11) NOT NULL,
  `relanceParTour` int(11) NOT NULL,
  `cdDomBonnus` int(11) NOT NULL DEFAULT '0',
  `element` varchar(10) NOT NULL DEFAULT '',
  `distanceFromMelee` int(5) NOT NULL DEFAULT '0' COMMENT 'distance la lancement de sort entre le joueur et l''adversaire',
  `owner` varchar(15) NOT NULL,
  `target` varchar(100) DEFAULT NULL COMMENT 'enemy_1/ally_1 ...',
  `typeEffect` varchar(100) DEFAULT NULL,
  `zoneEffect` varchar(25) NOT NULL,
  `sizeEffect` int(5) DEFAULT '0',
  `extraDataEffect` varchar(255) DEFAULT NULL COMMENT 'effect base # effect id # target(separed by / if many) # element # duraion # delay # zoneSize # zoneExtensible # handToHandDistance # min # max # flag1 # flag2 # flag3 | (separator between effects)',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=latin1 COMMENT='la grandeur de la zone d''effet ou glyph';

-- ----------------------------
-- Records of spells
-- ----------------------------
INSERT INTO `spells` VALUES ('1', '0', 'rasengan', '1', '4', '2', 'ninjutsu', 'a', '0', '1', '5', '8', '40', '1', '1', '1', '10', 'futon', '2', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '1', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#futon#0#0#1#false#2#5#8#0#0#0');
INSERT INTO `spells` VALUES ('2', '0', 'rasengan', '2', '4', '3', 'ninjutsu', 'a', '0', '1', '7', '10', '35', '1', '1', '2', '10', 'futon', '2', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '1', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#futon#0#0#1#false#2#7#10#0#0#0');
INSERT INTO `spells` VALUES ('3', '0', 'rasengan', '3', '4', '4', 'ninjutsu', 'a', '0', '1', '9', '12', '30', '1', '2', '2', '10', 'futon', '2', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '1', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#futon#0#0#1#false#2#9#12#0#0#0');
INSERT INTO `spells` VALUES ('4', '0', 'rasengan', '4', '4', '5', 'ninjutsu', 'a', '0', '1', '11', '14', '25', '1', '2', '3', '10', 'futon', '2', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '1', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#futon#0#0#1#false#2#11#14#0#0#0');
INSERT INTO `spells` VALUES ('5', '0', 'rasengan', '5', '4', '6', 'ninjutsu', 'a', '0', '1', '13', '16', '20', '1', '2', '3', '10', 'futon', '2', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '1', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#futon#0#0#1#false#2#13#16#0#0#0');
INSERT INTO `spells` VALUES ('6', '1', 'shuriken', '1', '4', '2', 'ninjutsu', 'a', '1', '1', '5', '8', '40', '1', '0', '2', '10', 'doton', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'oneTile', '1', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#1#false#0#5#8#0#0#0');
INSERT INTO `spells` VALUES ('7', '1', 'shuriken', '2', '4', '3', 'ninjutsu', 'a', '1', '1', '7', '10', '35', '1', '0', '2', '10', 'doton', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'oneTile', '1', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#1#false#0#7#10#0#0#0');
INSERT INTO `spells` VALUES ('8', '1', 'shuriken', '3', '4', '4', 'ninjutsu', 'a', '1', '1', '9', '12', '30', '1', '0', '2', '10', 'doton', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'oneTile', '1', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#1#false#0#9#12#0#0#0');
INSERT INTO `spells` VALUES ('9', '1', 'shuriken', '4', '4', '5', 'ninjutsu', 'a', '1', '1', '11', '14', '25', '1', '0', '2', '10', 'doton', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'oneTile', '1', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#1#false#0#11#14#0#0#0');
INSERT INTO `spells` VALUES ('10', '1', 'shuriken', '5', '4', '6', 'ninjutsu', 'a', '1', '1', '13', '16', '20', '1', '0', '2', '10', 'doton', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'oneTile', '1', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#1#false#0#13#16#0#0#0');
INSERT INTO `spells` VALUES ('11', '3', 'kage bunshin', '1', '4', '1', 'ninjutsu', 'b', '0', '0', '0', '0', '40', '4', '0', '1', '10', 'neutre', '0', 'naruto', 'none', 'allDirections', 'oneTile', '1', 'summon#0#none#neutre#0#0#0#false#0#0#0#100#1#0');
INSERT INTO `spells` VALUES ('12', '3', 'kage bunshin', '2', '4', '2', 'ninjutsu', 'b', '0', '0', '0', '0', '35', '4', '0', '1', '10', 'neutre', '0', 'naruto', 'none', 'allDirections', 'oneTile', '1', 'summon#0#none#neutre#0#0#0#false#0#0#0#101#1#0');
INSERT INTO `spells` VALUES ('13', '3', 'kage bunshin', '3', '4', '3', 'ninjutsu', 'b', '0', '0', '0', '0', '30', '4', '0', '1', '10', 'neutre', '0', 'naruto', 'none', 'allDirections', 'oneTile', '1', 'summon#0#none#neutre#0#0#0#false#0#0#0#102#1#0');
INSERT INTO `spells` VALUES ('14', '3', 'kage bunshin', '4', '4', '4', 'ninjutsu', 'b', '0', '0', '0', '0', '25', '4', '0', '1', '10', 'neutre', '0', 'naruto', 'none', 'allDirections', 'oneTile', '1', 'summon#0#none#neutre#0#0#0#false#0#0#0#103#1#0');
INSERT INTO `spells` VALUES ('15', '3', 'kage bunshin', '5', '4', '5', 'ninjutsu', 'b', '0', '0', '0', '0', '20', '4', '0', '1', '10', 'neutre', '0', 'naruto', 'none', 'allDirections', 'oneTile', '1', 'summon#0#none#neutre#0#0#0#false#0#0#0#104#1#0');
INSERT INTO `spells` VALUES ('16', '4', 'pounch', '1', '4', '1', 'taijutsu', 'd', '0', '1', '4', '6', '40', '1', '3', '5', '10', 'doton', '0', 'invoc naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '0', 'dammage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#0#false#0#4#6#0#0#0');
INSERT INTO `spells` VALUES ('17', '4', 'pounch', '2', '4', '1', 'taijutsu', 'd', '0', '1', '7', '9', '35', '1', '3', '5', '10', 'doton', '0', 'invoc naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '0', 'dammage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#0#false#0#7#9#0#0#0');
INSERT INTO `spells` VALUES ('18', '4', 'pounch', '3', '4', '1', 'taijutsu', 'd', '0', '1', '10', '12', '30', '1', '3', '5', '10', 'doton', '0', 'invoc naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '0', 'dammage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#0#false#0#10#12#0#0#0');
INSERT INTO `spells` VALUES ('19', '4', 'pounch', '4', '4', '1', 'taijutsu', 'd', '0', '1', '13', '15', '25', '1', '3', '5', '10', 'doton', '0', 'invoc naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '0', 'dammage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#0#false#0#13#15#0#0#0');
INSERT INTO `spells` VALUES ('20', '4', 'pounch', '5', '4', '1', 'taijutsu', 'd', '0', '1', '16', '18', '20', '1', '3', '5', '10', 'doton', '0', 'invoc naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'perpendicular', 'oneTile', '0', 'dammage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#0#false#0#16#18#0#0#0');
INSERT INTO `spells` VALUES ('21', '2', 'rasen shuriken', '1', '4', '2', 'ninjutsu', 'a', '1', '1', '5', '8', '40', '1', '0', '2', '10', 'futon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'rhombus', '3', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#3#false#2#5#8#0#0#0');
INSERT INTO `spells` VALUES ('22', '2', 'rasen shuriken', '2', '4', '3', 'ninjutsu', 'a', '1', '1', '7', '10', '35', '1', '0', '2', '10', 'futon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'rhombus', '3', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#3#false#2#7#10#0#0#0');
INSERT INTO `spells` VALUES ('23', '2', 'rasen shuriken', '3', '4', '4', 'ninjutsu', 'a', '1', '1', '9', '12', '30', '1', '0', '2', '10', 'futon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'rhombus', '3', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#3#false#2#9#12#0#0#0');
INSERT INTO `spells` VALUES ('24', '2', 'rasen shuriken', '4', '4', '5', 'ninjutsu', 'a', '1', '1', '11', '14', '25', '1', '0', '2', '10', 'futon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'rhombus', '3', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#3#false#2#11#14#0#0#0');
INSERT INTO `spells` VALUES ('25', '2', 'rasen shuriken', '5', '4', '6', 'ninjutsu', 'a', '1', '1', '13', '16', '20', '1', '0', '2', '10', 'futon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'allDirections', 'rhombus', '3', 'defaultDamage#1#enemy_1/ally_1/none/ally_summon/enemy_summon#doton#0#0#3#false#2#13#16#0#0#0');
INSERT INTO `spells` VALUES ('26', '5', 'gamabunta', '1', '4', '1', 'invocation', 'd', '0', '1', '4', '6', '40', '1', '1', '1', '10', 'katon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'melee', 'threeFrontalPerpendicular', '0', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#katon#0#0#0#false#0#4#6#0#0#0');
INSERT INTO `spells` VALUES ('27', '5', 'gamabunta', '2', '4', '1', 'invocation', 'd', '0', '1', '7', '9', '35', '1', '2', '2', '10', 'katon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'melee', 'threeFrontalPerpendicular', '0', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#katon#0#0#0#false#0#7#9#0#0#0');
INSERT INTO `spells` VALUES ('28', '5', 'gamabunta', '3', '4', '1', 'invocation', 'd', '0', '1', '10', '12', '30', '1', '2', '3', '10', 'katon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'melee', 'threeFrontalPerpendicular', '0', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#katon#0#0#0#false#0#10#12#0#0#0');
INSERT INTO `spells` VALUES ('29', '5', 'gamabunta', '4', '4', '1', 'invocation', 'd', '0', '1', '13', '15', '25', '1', '2', '3', '10', 'katon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'melee', 'threeFrontalPerpendicular', '0', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#katon#0#0#0#false#0#13#15#0#0#0');
INSERT INTO `spells` VALUES ('30', '5', 'gamabunta', '5', '4', '1', 'invocation', 'd', '0', '1', '16', '18', '20', '1', '3', '3', '10', 'katon', '0', 'naruto', 'enemy_1#ally_1#none#ally_summon#enemy_summon', 'melee', 'threeFrontalPerpendicular', '0', 'defaultDamage#0#enemy_1/ally_1/none/ally_summon/enemy_summon#katon#0#0#0#false#0#16#18#0#0#0');
INSERT INTO `spells` VALUES ('31', '6', 'transfert de vie', '1', '4', '2', 'ninjutsu', 'a', '1', '0', '0', '0', '40', '5', '1', '1', '10', 'neutre', '0', 'naruto', 'ally_summon', 'allDirections', 'plus', '0', 'killSummonToBoost#0#ally_summon#doton#0#0#0#false#0#0#0#1#health#$%:10');
INSERT INTO `spells` VALUES ('32', '6', 'transfert de vie', '2', '4', '3', 'ninjutsu', 'a', '1', '0', '0', '0', '35', '5', '1', '1', '10', 'neutre', '0', 'naruto', 'ally_summon', 'allDirections', 'plus', '0', 'killSummonToBoost#0#ally_summon#doton#0#0#0#false#0#0#0#1#health#$%:20');
INSERT INTO `spells` VALUES ('33', '6', 'transfert de vie', '3', '4', '4', 'ninjutsu', 'a', '1', '0', '0', '0', '30', '4', '1', '1', '10', 'neutre', '0', 'naruto', 'ally_summon', 'allDirections', 'plus', '0', 'killSummonToBoost#0#ally_summon#doton#0#0#0#false#0#0#0#1#health#$%:30');
INSERT INTO `spells` VALUES ('34', '6', 'transfert de vie', '4', '4', '5', 'ninjutsu', 'a', '1', '0', '0', '0', '25', '3', '1', '1', '10', 'neutre', '0', 'naruto', 'ally_summon', 'allDirections', 'plus', '0', 'killSummonToBoost#0#ally_summon#doton#0#0#0#false#0#0#0#1#health#$%:40');
INSERT INTO `spells` VALUES ('35', '6', 'transfert de vie', '5', '4', '6', 'ninjutsu', 'a', '1', '0', '0', '0', '20', '2', '1', '1', '10', 'neutre', '0', 'naruto', 'ally_summon', 'allDirections', 'plus', '0', 'killSummonToBoost#0#ally_summon#doton#0#0#0#false#0#0#0#1#health#$%:50');
INSERT INTO `spells` VALUES ('36', '7', 'transfert de pc', '1', '4', '2', 'ninjutsu', 'a', '1', '0', '0', '0', '40', '5', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('37', '7', 'transfert de pc', '2', '4', '3', 'ninjutsu', 'a', '1', '0', '0', '0', '35', '5', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('38', '7', 'transfert de pc', '3', '4', '4', 'ninjutsu', 'a', '1', '0', '0', '0', '30', '4', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('39', '7', 'transfert de pc', '4', '4', '5', 'ninjutsu', 'a', '1', '0', '0', '0', '25', '3', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('40', '7', 'transfert de pc', '5', '4', '6', 'ninjutsu', 'a', '1', '0', '0', '0', '20', '2', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('41', '8', 'transfert de pm', '1', '4', '2', 'ninjutsu', 'a', '1', '0', '0', '0', '40', '5', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('42', '8', 'transfert de pm', '2', '4', '3', 'ninjutsu', 'a', '1', '0', '0', '0', '35', '5', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('43', '8', 'transfert de pm', '3', '4', '4', 'ninjutsu', 'a', '1', '0', '0', '0', '30', '4', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('44', '8', 'transfert de pm', '4', '4', '5', 'ninjutsu', 'a', '1', '0', '0', '0', '25', '3', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('45', '8', 'transfert de pm', '5', '4', '6', 'ninjutsu', 'a', '1', '0', '0', '0', '20', '2', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('46', '9', 'transfert de puissance', '1', '4', '2', 'ninjutsu', 'a', '1', '0', '0', '0', '40', '5', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('47', '9', 'transfert de puissance', '2', '4', '3', 'ninjutsu', 'a', '1', '0', '0', '0', '35', '5', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('48', '9', 'transfert de puissance', '3', '4', '4', 'ninjutsu', 'a', '1', '0', '0', '0', '30', '4', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('49', '9', 'transfert de puissance', '4', '4', '5', 'ninjutsu', 'a', '1', '0', '0', '0', '25', '3', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('50', '9', 'transfert de puissance', '5', '4', '6', 'ninjutsu', 'a', '1', '0', '0', '0', '20', '2', '1', '1', '10', 'neutre', '0', 'naruto', null, null, 'plus', '0', null);
INSERT INTO `spells` VALUES ('51', '10', 'mode sennin', '1', '4', '0', 'ninjutsu', 'a', '0', '0', '0', '0', '40', '5', '1', '1', '10', 'doton', '0', 'naruto', null, null, 'self', '0', null);
INSERT INTO `spells` VALUES ('52', '10', 'mode sennin', '2', '4', '0', 'ninjutsu', 'a', '0', '0', '0', '0', '35', '5', '1', '1', '10', 'doton', '0', 'naruto', null, null, 'self', '0', null);
INSERT INTO `spells` VALUES ('53', '10', 'mode sennin', '3', '4', '0', 'ninjutsu', 'a', '0', '0', '0', '0', '30', '5', '1', '1', '10', 'doton', '0', 'naruto', null, null, 'self', '0', null);
INSERT INTO `spells` VALUES ('54', '10', 'mode sennin', '4', '4', '0', 'ninjutsu', 'a', '0', '0', '0', '0', '25', '5', '1', '1', '10', 'doton', '0', 'naruto', null, null, 'self', '0', null);
INSERT INTO `spells` VALUES ('55', '10', 'mode sennin', '5', '4', '0', 'ninjutsu', 'a', '0', '0', '0', '0', '20', '5', '1', '1', '10', 'doton', '0', 'naruto', null, null, 'self', '0', null);
INSERT INTO `spells` VALUES ('56', '11', 'katas des crapauds', '1', '4', '1', 'taijutsu', 'a', '0', '0', '5', '8', '40', '1', '1', '1', '10', 'doton', '0', 'naruto', null, null, 'diagonal', '0', null);
INSERT INTO `spells` VALUES ('57', '11', 'katas des crapauds', '2', '4', '1', 'taijutsu', 'a', '0', '0', '7', '10', '35', '1', '1', '2', '10', 'doton', '0', 'naruto', null, null, 'diagonal', '0', null);
INSERT INTO `spells` VALUES ('58', '11', 'katas des crapauds', '3', '4', '1', 'taijutsu', 'a', '0', '0', '9', '12', '30', '1', '2', '2', '10', 'doton', '0', 'naruto', null, null, 'diagonal', '0', null);
INSERT INTO `spells` VALUES ('59', '11', 'katas des crapauds', '4', '4', '1', 'taijutsu', 'a', '0', '0', '11', '14', '25', '1', '2', '3', '10', 'doton', '0', 'naruto', null, null, 'diagonal', '0', null);
INSERT INTO `spells` VALUES ('60', '11', 'katas des crapauds', '5', '4', '1', 'taijutsu', 'a', '0', '0', '13', '16', '20', '1', '2', '3', '10', 'doton', '0', 'naruto', null, null, 'diagonal', '0', null);

-- ----------------------------
-- Table structure for `state`
-- ----------------------------
DROP TABLE IF EXISTS `state`;
CREATE TABLE `state` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `state` varchar(15) NOT NULL,
  `detail` varchar(50) NOT NULL COMMENT 'maintenance ou restarting ou shutdown',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of state
-- ----------------------------
INSERT INTO `state` VALUES ('1', 'idle', '');

-- ----------------------------
-- Table structure for `summon`
-- ----------------------------
DROP TABLE IF EXISTS `summon`;
CREATE TABLE `summon` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `template_id` int(10) NOT NULL,
  `name` varchar(20) NOT NULL,
  `ClasseName` varchar(10) NOT NULL,
  `size` varchar(10) NOT NULL,
  `level` int(3) NOT NULL DEFAULT '1',
  `sexe` varchar(10) NOT NULL,
  `MaskColors` varchar(44) NOT NULL,
  `skinTemplate` varchar(255) NOT NULL DEFAULT '' COMMENT 'contiens un identifiant qui permet de choisir le skin',
  `TotalPdv` varchar(10) NOT NULL DEFAULT '0' COMMENT 'points de vie',
  `CurrentPdv` varchar(10) NOT NULL,
  `rang` int(10) NOT NULL DEFAULT '0',
  `xp` varchar(15) NOT NULL DEFAULT '0' COMMENT 'xp du niveau avant le up',
  `doton` varchar(10) NOT NULL DEFAULT '0',
  `katon` varchar(10) NOT NULL DEFAULT '0',
  `futon` varchar(10) NOT NULL DEFAULT '0',
  `raiton` varchar(10) NOT NULL DEFAULT '0',
  `suiton` varchar(10) NOT NULL DEFAULT '0',
  `puissance` varchar(10) NOT NULL DEFAULT '0',
  `usingDoton` varchar(10) NOT NULL DEFAULT '0',
  `usingKaton` varchar(10) NOT NULL DEFAULT '0',
  `usingFuton` varchar(10) NOT NULL DEFAULT '0',
  `usingRaiton` varchar(10) NOT NULL DEFAULT '0',
  `usingSuiton` varchar(10) NOT NULL DEFAULT '0',
  `dotonEquiped` varchar(10) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `katonEquiped` varchar(10) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `futonEquiped` varchar(10) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `raitonEquiped` varchar(10) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `suitonEquiped` varchar(10) NOT NULL DEFAULT '0' COMMENT 'points par les equipements',
  `puissanceEquiped` varchar(10) NOT NULL DEFAULT '0' COMMENT 'puissance dans les items utilisés',
  `pc` varchar(10) NOT NULL DEFAULT '5' COMMENT 'point de chakra',
  `pm` varchar(10) NOT NULL DEFAULT '3',
  `pe` varchar(10) NOT NULL DEFAULT '0',
  `cd` varchar(10) NOT NULL DEFAULT '0' COMMENT 'coup dangeureu cd',
  `invoc` varchar(10) NOT NULL DEFAULT '1',
  `initiative` varchar(10) NOT NULL DEFAULT '100',
  `resiDoton` varchar(10) NOT NULL DEFAULT '0',
  `resiKaton` varchar(10) NOT NULL DEFAULT '0',
  `resiFuton` varchar(10) NOT NULL DEFAULT '0',
  `resiRaiton` varchar(10) NOT NULL DEFAULT '0',
  `resiSuiton` varchar(10) NOT NULL DEFAULT '0',
  `resiDotonFix` varchar(10) NOT NULL DEFAULT '0',
  `resiKatonFix` varchar(10) NOT NULL DEFAULT '0',
  `resiFutonFix` varchar(10) NOT NULL DEFAULT '0',
  `resiRaitonFix` varchar(10) NOT NULL DEFAULT '0',
  `resiSuitonFix` varchar(10) NOT NULL DEFAULT '0',
  `resiDotonPercent` varchar(10) NOT NULL,
  `resiKatonPercent` varchar(10) NOT NULL,
  `resiFutonPercent` varchar(10) NOT NULL,
  `resiSuitonPercent` varchar(10) NOT NULL,
  `resiRaitonPercent` varchar(10) NOT NULL,
  `resiFix` varchar(10) NOT NULL DEFAULT '0',
  `esquivePC` varchar(10) NOT NULL DEFAULT '0',
  `esquivePM` varchar(10) NOT NULL DEFAULT '0',
  `esquivePE` varchar(10) NOT NULL DEFAULT '0',
  `esquiveCD` varchar(10) NOT NULL DEFAULT '0',
  `retraitPC` varchar(10) NOT NULL DEFAULT '0',
  `retraitPM` varchar(10) NOT NULL DEFAULT '0',
  `retraitPE` varchar(10) NOT NULL DEFAULT '0',
  `retraitCD` varchar(10) NOT NULL DEFAULT '0',
  `evasion` varchar(10) NOT NULL DEFAULT '0',
  `blocage` varchar(10) NOT NULL DEFAULT '0',
  `sorts` varchar(255) NOT NULL COMMENT 'sortID:emplacementID:level:couleurID|...',
  `domDotonFix` varchar(10) NOT NULL DEFAULT '0',
  `domKatonFix` varchar(10) NOT NULL DEFAULT '0',
  `domFutonFix` varchar(10) NOT NULL DEFAULT '0',
  `domRaitonFix` varchar(10) NOT NULL DEFAULT '0',
  `domSuitonFix` varchar(10) NOT NULL DEFAULT '0',
  `domFix` varchar(10) NOT NULL DEFAULT '0',
  `comment` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`),
  KEY `id_3` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of summon
-- ----------------------------
INSERT INTO `summon` VALUES ('1', '100', '$copy', '$copy', '0', '1', '$copy', '$copy', '', '$%:20', '', '0', '0', '100', '100', '100', '100', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '6', '4', '4', '10', '0', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '', '', '', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '4:0:1:0', '0', '0', '0', '0', '0', '0', 'double naruto');
INSERT INTO `summon` VALUES ('2', '101', '$copy', '$copy', '0', '2', '$copy', '$copy', '', '$%:30', '', '0', '0', '150', '150', '150', '150', '150', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '7', '4', '4', '10', '0', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '', '', '', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '4:0:2:0', '0', '0', '0', '0', '0', '0', 'double naruto');
INSERT INTO `summon` VALUES ('3', '102', '$copy', '$copy', '0', '3', '$copy', '$copy', '', '$%:40', '', '0', '0', '200', '200', '200', '200', '200', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '7', '5', '4', '10', '0', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '', '', '', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '4:0:3:0', '0', '0', '0', '0', '0', '0', 'double naruto');
INSERT INTO `summon` VALUES ('4', '103', '$copy', '$copy', '0', '4', '$copy', '$copy', '', '$%:50', '', '0', '0', '250', '250', '250', '250', '250', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '8', '5', '4', '10', '0', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '', '', '', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '4:0:4:0', '0', '0', '0', '0', '0', '0', 'double naruto');
INSERT INTO `summon` VALUES ('5', '104', '$copy', '$copy', '0', '5', '$copy', '$copy', '', '$%:60', '', '0', '0', '301', '302', '303', '304', '305', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '8', '6', '5', '10', '0', '100', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '', '', '', '', '', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '4:0:5:0', '0', '0', '0', '0', '0', '0', 'double naruto');

-- ----------------------------
-- Table structure for `users`
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `username` varchar(15) NOT NULL,
  `password` varchar(32) NOT NULL,
  `email` varchar(40) NOT NULL,
  `confirmation_lien_email` varchar(40) NOT NULL,
  `confirmation_email` tinyint(1) NOT NULL DEFAULT '0',
  `ville` varchar(20) NOT NULL,
  `pays` varchar(20) NOT NULL,
  `date_de_nessance` varchar(10) NOT NULL,
  `centre_d_interet` varchar(255) NOT NULL,
  `etat_compte` varchar(10) NOT NULL,
  `etat_compte_reson` varchar(255) NOT NULL,
  `statut_compte` varchar(20) NOT NULL,
  `vip` tinyint(1) NOT NULL DEFAULT '0',
  `premium` tinyint(1) NOT NULL DEFAULT '0',
  `abonnement` tinyint(1) NOT NULL DEFAULT '0',
  `fin_abonnement` varchar(22) NOT NULL,
  `question_secrette` varchar(25) NOT NULL,
  `reponse_secrette` varchar(25) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', 'admin', '202cb962ac59075b964b07152d234b70', '', '', '1', '', '', '', '', '', '', '', '0', '0', '0', '', 'ton chien favorie', 'vikiviki');
INSERT INTO `users` VALUES ('2', 'admin2', '202cb962ac59075b964b07152d234b70', '', '', '1', '', '', '', '', '', '', '', '0', '0', '0', '', 'ton chien favorie', 'vikiviki');
INSERT INTO `users` VALUES ('5', 'morpher', '202cb962ac59075b964b07152d234b70', '', '', '1', '', '', '', '', '', '', '', '0', '0', '0', '', 'ton chien favorie', 'vikiviki');
INSERT INTO `users` VALUES ('6', 'admin3', '202cb962ac59075b964b07152d234b70', '', '', '1', '', '', '', '', '', '', '', '0', '0', '0', '', 'ton chien favorie', 'vikiviki');
INSERT INTO `users` VALUES ('7', 'admin4', '202cb962ac59075b964b07152d234b70', '', '', '1', '', '', '', '', '', '', '', '0', '0', '0', '', 'ton chien favorie', 'vikiviki');

-- ----------------------------
-- Table structure for `version`
-- ----------------------------
DROP TABLE IF EXISTS `version`;
CREATE TABLE `version` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `app` varchar(10) NOT NULL,
  `version` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of version
-- ----------------------------
INSERT INTO `version` VALUES ('1', 'app', '1.1');

-- ----------------------------
-- Table structure for `xplevel`
-- ----------------------------
DROP TABLE IF EXISTS `xplevel`;
CREATE TABLE `xplevel` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `level` int(3) NOT NULL,
  `xp` int(20) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of xplevel
-- ----------------------------
INSERT INTO `xplevel` VALUES ('1', '1', '1000');
INSERT INTO `xplevel` VALUES ('2', '2', '2000');
INSERT INTO `xplevel` VALUES ('3', '3', '3000');
INSERT INTO `xplevel` VALUES ('4', '4', '4000');
INSERT INTO `xplevel` VALUES ('5', '5', '5000');
INSERT INTO `xplevel` VALUES ('6', '6', '6000');
INSERT INTO `xplevel` VALUES ('7', '7', '7000');
INSERT INTO `xplevel` VALUES ('8', '8', '8000');
INSERT INTO `xplevel` VALUES ('9', '9', '9000');
INSERT INTO `xplevel` VALUES ('10', '10', '10000');
INSERT INTO `xplevel` VALUES ('11', '11', '11000');
INSERT INTO `xplevel` VALUES ('12', '12', '12000');
INSERT INTO `xplevel` VALUES ('13', '13', '13000');
INSERT INTO `xplevel` VALUES ('14', '14', '14000');
INSERT INTO `xplevel` VALUES ('15', '15', '15000');
INSERT INTO `xplevel` VALUES ('16', '16', '16000');
INSERT INTO `xplevel` VALUES ('17', '17', '17000');
INSERT INTO `xplevel` VALUES ('18', '18', '18000');
INSERT INTO `xplevel` VALUES ('19', '19', '19000');
INSERT INTO `xplevel` VALUES ('20', '20', '20000');
INSERT INTO `xplevel` VALUES ('21', '21', '21000');
INSERT INTO `xplevel` VALUES ('22', '22', '22000');
INSERT INTO `xplevel` VALUES ('23', '23', '23000');
INSERT INTO `xplevel` VALUES ('24', '24', '24000');
INSERT INTO `xplevel` VALUES ('25', '25', '25000');
INSERT INTO `xplevel` VALUES ('26', '26', '26000');
INSERT INTO `xplevel` VALUES ('27', '27', '27000');
INSERT INTO `xplevel` VALUES ('28', '28', '28000');
INSERT INTO `xplevel` VALUES ('29', '29', '29000');
INSERT INTO `xplevel` VALUES ('30', '30', '40000');
