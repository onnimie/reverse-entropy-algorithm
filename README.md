# reverse-entropy-algorithm
Satunnaista kaksipuolueista pikselikarttaa kehitetään algoritmilla.

Luodaan täysin satunnaisesta pikselikartasta järjestäytyneitä muodostelmia proseduraalisesti algoritmillä, joka tarkastelee kunkin alkion naapureita, jotka puolestaan määräävät pikselin kehityksen.

Algoritmi (ritmi_2) käy läpi jokaisen pikselin ja tämän naapurit. Riippuen A-puolueisten naapurien määrästä, itse pikseli kehittyy joko A-puolueiseksi tai B-puolueiseksi (musta/valkoinen). Pikseli on aina jompaa kumpaa, ja alustuksessa kukin pikseli määräytyy puolueeseen satunnaisesti.
Ritmi_2 kehittää tällä tavalla koko pikselikarttaa ja muodostaa jokaisella iteraatiolla uuden pikselikartan kehittyneillä puolueilla. Täten pikselikarttaan muodostuu järkeviä muodostelmia satunnaisen lumisateen sijaan.
