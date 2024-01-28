import { FC, useState } from "react";

import { Navbar, Song, SongList } from "../../components";
import UserPage from "../UserPage";
import ArtistPage from "../ArtistPage";
import SongsPage from "../SongsPage";
import AlbumsPage from "../AlbumsPage";
import GenresPage from "../GenresPage";

import "./LandingPage.styles.scss";


const LandingPage: FC<{id:string|null}> = ({id}) => {
  const [ selected, setSelected ] = useState(0);

  const generateBody = () => {
    if(selected === 0) return <UserPage userId={id}/>
    if(selected === 1) return <ArtistPage/>
    if(selected === 2) return <SongsPage/>
    if(selected === 3) return <AlbumsPage/>
    if(selected === 4) return <GenresPage/>
  }

  return (
    <div className="landing-page__container">
      <Navbar
        selected={selected}
        setSelected={setSelected}
        className="landing-page__container__navbar"
        listItems={[{ label: "Username", value: "Stefanche" }]}
        id={id}
      />
      {generateBody()}
    </div>
  );
};

export default LandingPage;
