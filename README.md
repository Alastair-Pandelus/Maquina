# Maquina
Code Sample with Rules Engine triggering Domain Events

Contains the following elements :-

"Maquina" - (Spanish for machine) contains a generic rules Engine, this dynamically processes soft coded boolean logical expressions held in a formalised DSL that indirectly access business domains via reflection.

"Legatto" - (Italian for legacy) A hypothetical business domain containing an account based hierarchical document management system, this is held in memory only.

Legatto.CoreDomain.Test.AccountTest.BuildBulkAccount_Test() Unit test illustrates all elements of the example :-

  1) It creates a set of sample Legatto accounts, each with a randomised set of files and folders, an owner and optional 3rd party access accounts
  2) A couple of sample rules are set up to query the accounts, one looking for over-utilised accounts, one under utilised, this is notional and only illustrative, the output of the rules is a call to a message bus publish interface. This is stubbed only and displays a message in the debug console.
  3) The rules engine is invoked for all combinations of Account, User and Rule, triggered rules are shown in the Output / Debug console
  
  References :-
  
  1) Martin Fowler "Domain Specific Languages" - https://martinfowler.com/books/dsl.html
  2) Vaughn Vernon "Domain-Driven Design Distilled" - https://www.informit.com/store/domain-driven-design-distilled-9780134434988 

