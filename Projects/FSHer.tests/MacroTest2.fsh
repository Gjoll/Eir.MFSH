MacroDef: HeaderFragment
  * ^contact[0].telecom.system = http://hl7.org/fhir/contact-point-system#url
  * ^contact[0].telecom.value = "http://www.hl7.org/Special/committees/cic"

MacroDef: ObservationFragment1 "%a%" "%b%"
  * %a% 0..0
    Macro HeaderFragment
  * %b% 0..0

MacroDef: ObservationFragment2 "%a%" "%b%"
  * %a% 0..0
    Macro HeaderFragment
  * %b% 0..0

Profile: Test
Parent: Observation
Id: test
Title: "Test"
  * bodySite 1..1
  * bodySite = SNOMED#80248007 "Left breast structure (body structure)"
  Macro ObservationFragment1 "interpretation1" "referenceRange1"
  Macro ObservationFragment2 "interpretation2" "referenceRange2"
  * code = ObservationCodesCS#findingsLeftBreastObservation
