MacroDef: HeaderFragment
Parent: DiagnosticReport
  * ^contact[0].telecom.system = http://hl7.org/fhir/contact-point-system#url
  * ^contact[0].telecom.value = "http://www.hl7.org/Special/committees/cic"

Profile: Test
Parent: Observation
Id: test
Title: "Test"
  * bodySite 1..1
  Macro HeaderFragment
  * code = ObservationCodesCS#findingsLeftBreastObservation
