CREATE DATABASE  IF NOT EXISTS `cadastro` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `cadastro`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: cadastro
-- ------------------------------------------------------
-- Server version	5.6.38-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `materiaprima`
--

DROP TABLE IF EXISTS `materiaprima`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `materiaprima` (
  `Num` int(11) DEFAULT NULL,
  `Produto` varchar(30) DEFAULT 'Sem Produto',
  `Medida` int(11) DEFAULT '1'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materiaprima`
--

LOCK TABLES `materiaprima` WRITE;
/*!40000 ALTER TABLE `materiaprima` DISABLE KEYS */;
INSERT INTO `materiaprima` VALUES (1,'Sem Produto 1',1),(2,'Sem Produto 2',1),(3,'Sem Produto 3',1),(4,'Sem Produto 4',1),(5,'Sem Produto 5',1),(6,'Sem Produto 6',1),(7,'Sem Produto 7',1),(8,'Sem Produto 8',1),(9,'Sem Produto 9',1),(10,'Sem Produto 10',1),(11,'Sem Produto 11',1),(12,'Sem Produto 12',1),(13,'Sem Produto 13',1),(14,'Sem Produto 14',1),(15,'Sem Produto 15',1),(16,'Sem Produto 16',1),(17,'Sem Produto 17',1),(18,'Sem Produto 18',1),(19,'Sem Produto 19',1),(20,'Sem Produto 20',1),(21,'Sem Produto 21',1),(22,'Sem Produto 22',1),(23,'Sem Produto 23',1),(24,'Sem Produto 24',1),(25,'Sem Produto 25',1),(26,'Sem Produto 26',1),(27,'Sem Produto 27',1),(28,'Sem Produto 28',1),(29,'Sem Produto 29',1),(30,'Sem Produto 30',1),(31,'Sem Produto 31',1),(32,'Sem Produto 32',1);
/*!40000 ALTER TABLE `materiaprima` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `relatorio`
--

DROP TABLE IF EXISTS `relatorio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `relatorio` (
  `Dia` varchar(10) DEFAULT NULL,
  `Hora` time DEFAULT NULL,
  `Nome` varchar(30) DEFAULT NULL,
  `Form1` int(11) DEFAULT NULL,
  `Form2` int(11) DEFAULT NULL,
  `Prod_1` int(11) DEFAULT NULL,
  `Prod_2` int(11) DEFAULT NULL,
  `Prod_3` int(11) DEFAULT NULL,
  `Prod_4` int(11) DEFAULT NULL,
  `Prod_5` int(11) DEFAULT NULL,
  `Prod_6` int(11) DEFAULT NULL,
  `Prod_7` int(11) DEFAULT NULL,
  `Prod_8` int(11) DEFAULT NULL,
  `Prod_9` int(11) DEFAULT NULL,
  `Prod_10` int(11) DEFAULT NULL,
  `Prod_11` int(11) DEFAULT NULL,
  `Prod_12` int(11) DEFAULT NULL,
  `Prod_13` int(11) DEFAULT NULL,
  `Prod_14` int(11) DEFAULT NULL,
  `Prod_15` int(11) DEFAULT NULL,
  `Prod_16` int(11) DEFAULT NULL,
  `Prod_17` int(11) DEFAULT NULL,
  `Prod_18` int(11) DEFAULT NULL,
  `Prod_19` int(11) DEFAULT NULL,
  `Prod_20` int(11) DEFAULT NULL,
  `Prod_21` int(11) DEFAULT NULL,
  `Prod_22` int(11) DEFAULT NULL,
  `Prod_23` int(11) DEFAULT NULL,
  `Prod_24` int(11) DEFAULT NULL,
  `Prod_25` int(11) DEFAULT NULL,
  `Prod_26` int(11) DEFAULT NULL,
  `Prod_27` int(11) DEFAULT NULL,
  `Prod_28` int(11) DEFAULT NULL,
  `Prod_29` int(11) DEFAULT NULL,
  `Prod_30` int(11) DEFAULT NULL,
  `Prod_31` int(11) DEFAULT NULL,
  `Prod_32` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `relatorio`
--

LOCK TABLES `relatorio` WRITE;
/*!40000 ALTER TABLE `relatorio` DISABLE KEYS */;
/*!40000 ALTER TABLE `relatorio` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-01-22  9:55:20
