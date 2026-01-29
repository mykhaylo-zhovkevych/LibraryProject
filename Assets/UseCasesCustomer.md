## Use Cases

### Use Case 1: Kundenkonto registrieren

**Beschreibung:** Als Kunde möchte ich mein Bibliothekskonto eröffnen lassen, damit ich mich später einloggen kann.  
**Vorbedingungen:** Der Kunde ist nicht bereits registriert.

**Ablauf:**
1. Kunde navigiert zu "Registrierung" Fenstern.
2. Kunde gibt folgende Daten ein.
    - Konto Name
    - Passwort
    - Email (optional)
3. System validiert die Eingaben  
    Bedingung 1 Wenn Pflichtfelder fehlen oder die Regeln nicht erfüllt sind, zeigt das System eine Fehlermeldung und speichert nichts.  
    Bedingung 2 Wenn der Kontoname bereits registriert ist, zeigt das System eine Fehlermeldung.
4. System speichert die Daten und leitet den Kunden zur Login-Seite weiter.

**Akzeptanzkriterien**
1. Das Konto ist im System gespeichert.
2. Der Kunde kann sich anschliessend einloggen.

--- 
### Use Case 2: Kundenlogin

**Beschreibung:** Als Kunde möchte ich mich anmelden, damit ich bequem die Bibliotheksinfrastruktur nutzen kann.  
**Vorbedingungen:** Kundenkonto existiert und ist aktiv.

**Ablauf:**
1. Kunde navigiert zu "Login" Fenstern.
2. Kunde gibt die Daten ein.
    - Konto Name
    - Passwort
3. System validiert die Eingaben.
4. System authentifiziert den Kunden.  
    Bedingung 1 Wenn Benutzername oder Passwort falsch sind, wird der Login abgelehnt und eine Fehlermeldung angezeigt.  
    Bedingung 2 Wenn das Konto gesperrt oder deaktiviert ist, wird der Login abgelehnt und eine Fehlermeldung angezeigt.
5. System leitet den Kunden zur Dashboard Fenstern weiter.

**Akzeptanzkriterien**
1. Der Kunde kann sich mit gültigen Daten anmelden.
2. Ungültige Daten verhindern den Login und zeigen eine Fehlermeldung.
---

### Use Case 3: Artikel ausleihen

**Beschreibung:** Als Kunde möchte ich einen Artikel ausleihen, damit ich ihn in meinem Besitz haben kann.

**Ablauf:**
1. Kunde navigiert zu "Katalog" Fenstern.
2. Kunde wählt spezifische Artikel.
    1. System erstellt die Ausleihe Anfrage(Popup).
    2. System prüft Verfügbarkeit und Richtlinie.
    3. System erstellt die Ausleihe.
    4. System aktualisiert die Verfügbarkeit in der UI.

Bedingung 1 Wenn kein Exemplar verfügbar ist, wird keine Ausleihe erstellt und eine Fehlermeldung angezeigt.  
Bedingung 2 Wenn keine gültige Richtlinie für den Benutzertyp und Artikeltyp existiert, wird keine Ausleihe erstellt und eine Fehlermeldung angezeigt.

**Akzeptanzkriterien**
1. Bei Erfolg erscheint das Artikelexemplar unter "Ausleihen und Reservierungen" Fenstern.
2. Bei Fehlern wird nichts gespeichert und eine passende Meldung angezeigt.

---

### Use Case 3.1: Artikel zurückgeben

**Beschreibung:** Als Kunde möchte ich einen ausgeliehenen Artikel zurückgeben, damit ich ihn nicht mehr ausgeliehen habe.  
**Vorbedingung:** Kunde ist eingeloggt und hat aktive Ausleihen.

**Ablauf:**
1. Kunde navigiert zu "Ausleihen und Reservierungen" Fenstern.
2. Kunde wählt eine aktive Ausleihe aus.
3. Kunde bestätigt Rückgabe.
4. System beendet die Ausleihe.
5. System aktualisiert die Verfügbarkeit und setzt in der UI den Status auf "Returniert".

Bedingung 1 Wenn die Ausleihe nicht mehr existiert oder bereits beendet ist, wird die Taste deaktiviert.

**Akzeptanzkriterien**
1. Der Artikel ist nach der Rückgabe als "Returniert" markiert    
2. Die Verfügbarkeit wird in der UI aktualisiert.

---

### Use Case 3.3 Artikel reservieren

**Beschreibung:** Als Kunde möchte ich einen Artikel reservieren, wenn er nicht verfügbar ist, damit ich ihn später ausleihen kann.

**Ablauf:**
1. Kunde navigiert zu "Katalog" Fenstern.
2. Kunde wählt einen Artikel aus.
    1. System prüft, ob eine Reservierung möglich ist.
    2. System speichert die Reservierung.

Bedingung 1 Wenn bereits eine Reservierung existiert, wird eine neue erstellt, aber für das nächste Artikelexemplar.  
Bedingung 2 Wenn bereits alle Artikelexemplare reserviert sind, wird eine Fehlermeldung gezeigt.

**Akzeptanzkriterien**  
2. Bei Erfolg erscheint das Artikelexemplar unter "Ausleihen und Reservierungen" Fenstern.  
3. Reservierungen werden gespeichert und sind in der UI sichtbar.

---
### Use Case 4: Katalog anzeigen

**Beschreibung:** Als Kunde möchte ich den Katalog sehen, damit ich verfügbare Artikel durchsuchen kann.

**Ablauf:**
1. Kunde navigiert zu "Katalog".
2. System zeigt eine Liste von allen Artikeln an.

Bedingung 1 Wenn keine Artikel existieren, zeigt die UI nichts.

**Akzeptanzkriterien**
1. Die UI zeigt eine Artikelliste mit Basisinformationen, wie Name, Genre, Autor etc.

---

### Use Case 4.1: Suche nach Titel

**Beschreibung:**  
Als Kunde möchte ich nach Titel suchen, damit ich einen bestimmten Artikel schnell finde.

**Ablauf:**
1. Kunde navigiert zu "Katalog" Fenstern.    
2. Kunde gibt einen Titel ins Suchfeld ein.
3. System filtert die Liste nach passenden Titeln.

Bedingung 1 Wenn keine Artikel passen, zeigt die UI eine leere Liste.

**Akzeptanzkriterien**
1. Die Liste wird anhand des Titels korrekt gefiltert.

---

### Use Case 4.2: Filter nach Genre

**Beschreibung:**  
Als Kunde möchte ich nach Genre filtern, damit ich passende Artikel schneller finde.

**Ablauf:**
1. Kunde navigiert zu "Katalog" Fenstern.
2. Kunde wählt ein Genre aus.
3. System zeigt nur Artikel dieses Genres an.

Bedingung 1 Wenn keine Artikel im Genre existieren, zeigt die UI eine leere Liste.

**Akzeptanzkriterien**
1. Der Genre Filter reduziert die Liste korrekt.

---

### Use Case 4.3: Filter nach Jahr

**Beschreibung:**  
Als Kunde möchte ich nach Erscheinungsjahr filtern, damit ich Artikel aus einer bestimmten Zeitspanne finde.

**Ablauf:**
1. Kunde navigiert zu "Katalog" Fenstern.
2. Kunde gibt ein Jahr ein.
3. System zeigt nur passende Artikel.

Bedingung 1 Wenn die Eingabe kein gültiges Jahr ist, zeigt die UI eine leere Liste.

**Akzeptanzkriterien**
1. Die Filterung nach Jahr funktioniert korrekt.
2. Ungültige Eingaben werden abgefangen.

---

### Use Case 5 Kontoverwaltung

**Beschreibung:** Als Kunde möchte ich ein Verwaltungspanel haben, damit ich mein Konto selbstständig verwalten kann.

**Ablauf:**
1. Kunde navigiert zu "Profil“ Fenstern.
2. Kunde kann:
    - Die Kontoinformationen aktualisieren.
        - Konto Name
        - Email
3. Kunde bestätigt die aktualisierten Daten.
4. System speichert die aktualisierten Daten und zeigt sie in der UI an.

Bedingung 1 Wenn die Daten kein gültiges Format haben oder Regeln verletzen, wird eine Fehlermeldung gezeigt.

**Akzeptanzkriterien**
1. Das System aktualisiert die UI mit neuen Kontoinformationen.

---

### Use Case 6: Übersicht über Ausleihen und Reservierungen

**Beschreibung:** Als Kunde möchte ich das Fenster "Ausleihen und Reservierungen" öffnen, damit ich meine Ausleihen und Reservierungen einsehen kann.

**Ablauf:**
1. Kunde navigiert zu "Ausleihen und Reservierungen" Fenstern.
2. System lädt alle relevanten Einträge des Kunden.
3. Kunde wählt eine gewünschte Ansicht aus.

Bedingung 1 Wenn der Kunde weder Ausleihen noch Reservierungen besitzt, zeigt das System eine leere Liste.

**Akzeptanzkriterien**
1. Die UI zeigt eine Übersicht.
2. Die UI hat Auswahlmöglichkeiten "Aktiv" und "Inaktiv" für die Anzeige der Ausleihen.

### Use Case 6.1: Aktive Ausleihen einsehen

**Beschreibung:** Als Kunde möchte ich meine aktiven Ausleihen sehen, damit ich weiss, welche Artikel ich aktuell ausgeliehen habe.

**Ablauf:**
1. Kunde navigiert zu "Ausleihen und Reservierungen" Fenstern.
2. Kunde wählt "Aktive".
3. System lädt alle aktiven Ausleihen des Kunden.
4. System zeigt die Liste an.

Bedingung 1 Wenn keine aktiven Ausleihen existieren, zeigt das System eine leere Liste.

**Akzeptanzkriterien**
1. Die UI zeigt nur aktive Ausleihen an.
2. Wenn keine aktiven Ausleihen existieren, wird eine leere Liste angezeigt.

---

### Use Case 6.2: Inaktive Ausleihen einsehen

**Beschreibung:** Als Kunde möchte ich meine inaktiven Ausleihen sehen, damit ich meine Ausleihhistorie sehen kann.

**Ablauf:**
1. Kunde navigiert zum Fenster "Ausleihen und Reservierungen".
2. Kunde wählt "Inaktive".
3. System lädt alle inaktiven Ausleihen des Kunden.
4. System zeigt die Liste an.

Bedingung 1 Wenn keine inaktiven Ausleihen existieren, zeigt das System eine leere Liste.

**Akzeptanzkriterien**
1. Die UI zeigt nur inaktive Ausleihen an.
2. Wenn keine inaktiven Ausleihen existieren, wird eine leere Liste angezeigt.