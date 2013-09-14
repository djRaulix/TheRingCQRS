Feature: Create retrieve and save aggregates
	In order to persist my business layer
	As Domain layer requester
	I want to be able to create retrieve by id and update my business objects 

@mytag
Scenario: Create new aggregate
	Given a Guid
	When i want a new aggregate
	Then a new aggregate with the given guid should ne retturned
