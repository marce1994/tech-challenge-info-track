# Dev Candidate Test 
InfoTrack provides a settlement service whereby a property purchaser's conveyancer meets with 
representatives of the mortgage provider and the vendor's conveyancer at an agreed time. InfoTrack 
has fixed capacity and can only provide a limited number of simultaneous settlements. 

We would like you to implement a booking API that will accept a booking time and respond indicating 
whether the reservation was successful or not. 

## Technical requirements
- Assume that all bookings are for the same day (do not worry about handling dates)
- InfoTrack's hours of business are 9am-5pm, all bookings must complete by 5pm (latest booking
is 4:00pm)
- Bookings are for 1 hour (booking at 9:00am means the spot is held from 9:00am to 9:59am)
- InfoTrack accepts up to 4 simultaneous settlements
- API needs to accept POST requests of the following format:
```json
{
  "bookingTime": "09:30",
  "name":"John Smith"
}
```
- Successful bookings should respond with an OK status and a booking Id in GUID form
```json
{
  "bookingId": "d90f8c55-90a5-4537-a99d-c68242a6012b"
}
```
- Requests for out of hours times should return a Bad Request status
- Requests with invalid data should return a Bad Request status
- Requests when all settlements at a booking time are reserved should return a Conflict status
- The name property should be a non-empty string
- The bookingTime property should be a 24-hour time (00:00 - 23:59)
- Bookings can be stored in-memory, it is fine for them to be forgotten when the application is
restarted
## Deliverable
- A link to your source control system containing your API code
- Instructions on the endpoint to contact when we run your solution
- Any additional details you feel are necessary to execute or understand your solution
- Target 1-2 hours on this solutio
