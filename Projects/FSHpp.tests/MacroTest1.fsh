RuleSet: HeaderFragment
  * ^contact[0].telecom.system = http://hl7.org/fhir/contact-point-system#url
  * ^contact[0].telecom.value = "http://www.hl7.org/Special/committees/cic"

RuleSet: ObservationFragment
  * interpretation 0..0
    macro HeaderFragment
  * referenceRange 0..0
