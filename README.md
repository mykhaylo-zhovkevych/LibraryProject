## Ziel der Software

- **Ziel:** Eine Bibliotheks-App, die sowohl für Administration als auch für Kunden nutzbar ist.
- **Plattformen:** Desktop-App (Avalonia, MVVM) und Web-App (Blazor).
- **Datenhaltung:** lokal gespeichert, ggf. persistent (z.B. Datei oder SQLite).
- **Kernlogik**: Alle Bibliotheksfunktionen in einem **Core-Projekt** definiert und unabhängig von UI.

---
## Voraussetzungen

1. Die bestehende Codebasis für dieses Projekt übernehmen und drauf aufbauen.
2. Das System muss zwei Interfaces haben: eines für die Bibliotheksadministration und eines für Kunden.
3. Die Daten können lokal gespeichert werden; optional soll eine persistente Datenspeicherung umgesetzt werden.
4. Der Code muss statisch typisiert sein und den Clean-Code-Konventionen folgen.

---

## Benutzergruppen & Rollen

Admin
- Artikeln hinzufügen, bearbeiten, löschen
- Benutzer verwalten (registrieren, sperren, reaktivieren)
- Ausleihhistorie überwachen

Kunde
- Artikeln ausleihen, zurückgeben, reservieren
- Artikel suchen (mit Erweiterungen)
- Eigenes Konto verwalten (Konto schliessen)
- Ausleihhistorie einsehen (mit Erweiterungen)

---

## Use Cases
_Einige Use Cases können aus einer Benutzergruppen  zu andere Benutzergruppen mitgenommen und implementiert werden._ 
- [Admin Use Cases](./Assets/UseCasesAdmin.md)
- [Customer Use Cases](./Assets/UseCasesCustomer.md)
---

## Arbeitsplan und Arbeitspakete mit Aufwandsschätzungen

| Datum                   | Arbeitspaket                        | Erwartete Aufgabe                           | Erledigte Aufgabe                                         |
| ----------------------- | ----------------------------------- | ------------------------------------------- | --------------------------------------------------------- |
| 05.01.2026 / 06.01.2026 | Vorbereitung und Konzept erstellung | Das fertig Konzept                          | Das Konzept wurden während Projekt Ausführung nachgefasst |
| 07.01.2026 / 09.01.2026 | Projektinitialisierung              | Die Core Projekt und allfällige Remastering | Ähnlich wie in Arbeitspaket 1 aber in geringem Masse      |
| 12.01.2026 / 26.01.2026 | Desktop App Implementierung         | Das minimalistisch UI                       | Ja                                                        |
| 26.01.2026 / 02.02.2026 | Web App Implementierung             | Das minimalistisch UI und RWD               |                                                           |
| 02.02.2026 / 03.02.2026 | Refactoring + UnitTesting           | Korrekte Tests Ergebnisse                   |                                                           |

---
## Architekturkonzept

### Clean Architecture

[![Design Application using Clean Architecture - DEV Community](https://media2.dev.to/dynamic/image/width=1000,height=420,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Forahufsvjymejio3beu7.png)

**Beschreibung** Das gesamte Projekt ist in vier Sichten (Layer) unterteilt. Jede Sicht hat eine klar definierte Verantwortung und darf nicht direkt mit anderen Sichten kommunizieren, sondern nur horizontal über definierte Schnittstellen.
Dies ermöglicht eine bessere Testbarkeit, Wartbarkeit und Erweiterbarkeit der Anwendung.

1. Domain Layer
2. Application Layer
3. Presentation Layer
4. Infrastructure Layer

**Vor -und Nachteile**

1. Klare Trennung der Verantwortlichkeiten
   Jede Schicht hat eine eindeutige Aufgabe: Die Domain ist für Geschäftslogik und Entitäten zuständig. Der Application Layer steuert Use Cases und den Ablauf der Anwendung. Die Infrastructure kümmert sich um Datenhaltung, Datenbanken und externe Services. Die Presentation ist für die Benutzeroberfläche verantwortlich.
2. Einfache Testbarkeit
   Unit-Tests können einfach umgesetzt werden, da die Infrastructure-Schicht durch Mock- oder Fake Implementierungen ersetzt werden kann.
3. Wartbarkeit
   Neue Features können hinzugefügt werden, ohne andere Layer stark zu verändern.
4. Saubere Abhängigkeiten
   Die Domain kennt die Infrastructure nicht, und die Application kennt die UI nicht.
   Dadurch entsteht eine lose Kopplung zwischen den einzelnen Teilen des Codes.

5. Setup Komplexität
   Der initiale Aufbau benötigt mehr Zeit, insbesondere bei kleinen oder mittelgrossen Projekten.
6. Event Hadling / Cross-Cutting Concerns
   Querschnittsthemen wie Logging, Validierung oder Security sind komplexer umzusetzen.

**Warum diese Architektur wurden ausgewählt?**
Ich habe diese Architektur ausgewählt, weil das passt für die kleine bis mitlernen Projekten die eine gemeinsamen Geschäftslogik haben müssen und die Trennung zwischen Sichten ermöglichen flexiblem Erwartbarkeit ohne die Kernlogik zu verändern müssen.

---
## Datenkonzept und High Level Design

## UX Design Prototype

https://drift-shed-51908381.figma.site




## Testprotokoll

| Modul | Aufgabe          | Verantwortung Aufgabe | Ergebnis      | Beteiligt an der Ergebniserstellung |
| ----- | ---------------- | --------------------- | ------------- | ----------------------------------- |
| Tests | Test durchführen | Testverantwortlicher  | Testprotokoll | Tester                              |
|       |                  |                       |               |                                     |