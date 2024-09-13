# Szoftverspecifikáció


## Projekt adatok
|                  |                                               |                                                                   |
|------------------|-----------------------------------------------|-------------------------------------------------------------------|
| **Projekt neve** | Állatorovosi nyilvántartó rendszer            |                                                                   |
| **Téma**         | Backend és Fronted                            | A szoftvernek várhatóan nagyobb kitettsége ezen két tantárgy felé |
| **Témavezető**   | Méhes József                                  | A szoftvernek várhatóan nagyobb kitettsége ezen két tantárgy felé |
| **Fő platform**  | Web                                           |                                                                   |
| **Osztály**      | 13T-II                                        |                                                                   |
| **Csoporttagok** | Dékány Csaba                                  | Boros Dániel                                                      |

## Tartalomjegyzék
<!-- TOC -->
* [Szoftverspecifikáció](#szoftverspecifikáció)
  * [Projekt adatok](#projekt-adatok)
  * [Tartalomjegyzék](#tartalomjegyzék)
  * [1. Bevezetés](#1-bevezetés)
  * [2. Funkcionális követelmények](#2-funkcionális-követelmények)
  * [3. Külső interfész követelményei](#3-külső-interfész-követelményei)
  * [4. Nem funkcionális követelmények](#4-nem-funkcionális-követelmények)
    * [4.1 Biztonság](#41-biztonság)
      * [4.1.1 Adatbiztonság](#411-adatbiztonság)
      * [4.1.2 Hitelesítés és jogosultságkezelés](#412-hitelesítés-és-jogosultságkezelés)
      * [4.1.3 Audit naplók](#413-audit-naplók)
      * [4.1.4 Mobil biztonság](#414-mobil-biztonság)
  * [5. Definiciók és rövidítések](#5-definiciók-és-rövidítések)
  * [6. További dokumentumok](#6-további-dokumentumok)
<!-- TOC -->

## 1. Bevezetés

## 2. Funkcionális követelmények

## 3. Külső interfész követelményei

## 4. Nem funkcionális követelmények
### 4.1 Biztonság

#### 4.1.1 Adatbiztonság
Minden érzékeny információt, kifejezetten ami a felhasználó azonosítására alkalmas titkosított formában kell eltárolni az adatbázisban

#### 4.1.2 Hitelesítés és jogosultságkezelés
![](https://ekran_site_uploads.storage.googleapis.com/wp-content/uploads/2024/05/23005257/1-Role-based-Access-Control.svg)
Biztonságos bejelentkezési módszerek (felhasználónév/jelszó) használata kétfaktoros hitelesítési lehetőséggel és jelszó nélküli mobilos belépési opcióval. Szerepkör-alapú hozzáférés-vezérlés (RBAC) megvalósítása a felhasználók számára a megfelelő adathozzáférés érdekében. 
> Passwordless bejelentkezés és a kétfaktoros hitelesítés opcionális 

#### 4.1.3 Audit naplók
Nyilvántartást kell vezetni a rendszerhozzáférésekről és az adatok módosításáról, különösen az orvosi és tulajdonosi adatok tekintetében.

#### 4.1.4 Mobil biztonság
Biztosítani kell a mobil alkalmazások és a háttérrendszer közötti biztonságos kommunikációt, beleértve az adatok titkosítását és az eszközök biztonságát.

## 5. Definiciók és rövidítések
|                  |                                                                                  |
|------------------|----------------------------------------------------------------------------------|
| **SRS**          | Software Requirment Specification                                                |
| **Páciens**      | A szoftver adatbázisában szereplő állat                                          |
| **Tulajdonos**   | A szoftver adatbázisában szereplő állat tulajdonosa                              |
| **Munkavállaló** | A szoftver adatbázisában szereplő "employee" szerepkörrel rendelkező felhasználó |
| RBAC             | Role-based access control                                                        |


## 6. További dokumentumok
