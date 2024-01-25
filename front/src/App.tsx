import InputLabel from "@mui/material/InputLabel";
import { useState } from "react";

import { UsernamePage, LandingPage } from "./pages";

import "./App.css";

function App() {
  const [username, setUsername] = useState<null | String>(null);
  const [id, setId] = useState<null | String>(null);


  return (
    <div className="App">
      {username ? <LandingPage id={id}/> : <UsernamePage setUsername={setUsername} setId = {setId} />}
    </div>
  );
}

export default App;
