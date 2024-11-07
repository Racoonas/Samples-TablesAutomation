Feature: AngularSortedTableTests

Background: 
	Given I Navigate to Angular tables page
	And I wait until Angular tables page fully loaded

@Regression
Scenario: SortingByNameTableTest
	Then Sorted table is displayed
	When I click on 'Name' column header in Sorted Table
	Then Rows are sorted by 'Name' column in 'ascending' order
	When I click on 'Name' column header in Sorted Table
	Then Rows are sorted by 'Name' column in 'descending' order

@Regression
Scenario: SortingByWeightTableTest
	Then Sorted table is displayed
	When I click on 'Weight' column header in Sorted Table
	Then Rows are sorted by 'Weight' column in 'ascending' order
	When I click on 'Weight' column header in Sorted Table
	Then Rows are sorted by 'Weight' column in 'descending' order
	
	
	