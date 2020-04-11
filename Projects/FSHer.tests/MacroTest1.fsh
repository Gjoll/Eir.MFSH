MacroDef: HeaderFragment
  * ^contact[0].telecom.system = http://hl7.org/fhir/contact-point-system#url
  * ^contact[0].telecom.value = "http://www.hl7.org/Special/committees/cic"

MacroDef: ObservationFragment
  * interpretation 0..0
    Macro HeaderFragment
  * referenceRange 0..0

Profile: Test
Parent: Observation
Id: test
Title: "Test"
  * bodySite 1..1
  * bodySite = SNOMED#80248007 "Left breast structure (body structure)"
  Macro ObservationFragment
  * code = ObservationCodesCS#findingsLeftBreastObservation
