Feature: AngularBasicTableTests

Background: 
	Given I Navigate to Angular tables page
	And I wait until Angular tables page fully loaded

@Regression
Scenario: TableShouldHaveCorrectHeaders
	Then Basic table is displayed
	And Basic table has following columns headers:
	| Header |
	| No.    |
	| Name   |
	| Weight |
	| Symbol |

@Regression
Scenario: TableShouldHaveSpecificRows
	Then Basic table contains following rows:
	| No. | Name      | Weight | Symbol |
	| 1   | Hydrogen  | 1.0079 | H      |
	| 4   | Beryllium | 9.0122 | Be     |

@Regression
Scenario: FirstRowShouldBeCorrect
	Then Row number '1' in Basic Table contains following values:
	| No. | Name      | Weight | Symbol |
	| 1   | Hydrogen  | 1.0079 | H      |

@Regression
Scenario: TableShouldHaveCorrectNumberOfRows
	Then Basic table has '10' rows in Manage Builders page
	