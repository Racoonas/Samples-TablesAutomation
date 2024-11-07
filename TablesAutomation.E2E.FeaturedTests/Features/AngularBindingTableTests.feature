Feature: AngularBindingTableTests

Background: 
	Given I Navigate to Angular tables page
	And I wait until Angular tables page fully loaded

@Regression
Scenario: ClickingFirstRowTest	
	Then Binding table is displayed
	When I click on the following row in Binding table
	| No. | Name     | Symbol |
	| 1   | Hydrogen | H      |
	Then Click Log contains following text:
	"""
	Clicked on Hydrogen
	"""
	
	
	