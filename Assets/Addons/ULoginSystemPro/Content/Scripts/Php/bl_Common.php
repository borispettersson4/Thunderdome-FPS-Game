<?php
$GameVersion = "1.5";
$hostName    = 'fdb23.awardspace.net';
$dbName      = '3148713_psn3';
$dbUser      = '3148713_psn3';
$dbPassworld = 'Eck6Ji9PVrHcCN9';
$secretKey   = "123456";
$base_url    = 'http://www.morfos.onlinewebshop.net/morfos/php/';
$emailFrom   = 'borisruiz4@gmail.com';
$GameName    = "PSN3";

function dbConnect()
{
    global $dbName;
    global $secretKey;
    global $hostName;
    global $dbUser;
    global $dbPassworld;
    
    $link = mysqli_connect($hostName, $dbUser, $dbPassworld, $dbName);
    
    if (!$link) {
        fail("Couldn´t connect to database server");
    }
    
    return $link;
}

function TrydbConnect()
{
    global $dbName;
    global $secretKey;
    global $hostName;
    global $dbUser;
    global $dbPassworld;
    
    $link = @mysqli_connect($hostName, $dbUser, $dbPassworld, $dbName) or die("2");
    return $link;
}

function safe($variable)
{
    $variable = addslashes(trim($variable));
    return $variable;
}

function fail($errorMsg)
{
    print $errorMsg;
    exit;
}

?>