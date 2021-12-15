# Rocket_Elevators_REST_API

In order to connect our information system to the equipment in operation throughout the territory served, we developed a REST API using C # and .NET Core to allow us to know and to manipulate the status of all the relevant entities of the operational database.

NOTE: for PATCH, update status (or any single field) using the following format:
                 [{"op": "replace", "path": "/status", "value": "Offline"}]

#### Postman Link: https://www.getpostman.com/collections/30515379c2a8fcfcb4f0
#### Link to the deployed version: https://rocket-elevator-rest.azurewebsites.net/api



To get batteries: https://rocketelevatorrestportal.azurewebsites.net/api/customers/claudie@cronin.name/batteries

To get columns: https://rocketelevatorrestportal.azurewebsites.net/api/customers/claudie@cronin.name/columns

To get elevators: https://rocketelevatorrestportal.azurewebsites.net/api/customers/claudie@cronin.name/elevators

