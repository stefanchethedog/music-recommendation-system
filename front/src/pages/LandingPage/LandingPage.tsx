import { FC, useEffect, useState } from "react";
import Button from "@mui/material/Button";
import axios from 'axios'

import { GET_LIKED_SONGS, GET_RECOMMENDED_SONGS } from "../../endpoints";
import { Navbar, Song, SongList } from "../../components";

import "./LandingPage.styles.scss";
import { SongProps } from "../../components/Song/Song.component";


const LandingPage: FC<{id:String|null}> = ({id}) => {
  const [ songs, setSongs ] = useState<Omit<SongProps[], 'className'>>();
  const [ likedSongs, setLikedSongs ] = useState<Omit<SongProps[], 'className'>>();

  const handleLoadRecommendedSongs = async() =>{
    if (id)
      await axios
        .get(GET_RECOMMENDED_SONGS(id))
        .then((res) => {
          setSongs(res.data);
        })
        .catch((err) => {
          console.log(err);
        });
  }

  const handleLoadLikedSongs = async() =>{
    if (id)
      await axios
        .get(GET_LIKED_SONGS(id))
        .then((res) => {
          setLikedSongs(res.data);
        })
        .catch((err) => {
          console.log(err);
        });
  }

  useEffect(()=>{
    const load = async () =>{
      await handleLoadRecommendedSongs()
      await handleLoadLikedSongs()
    }
    load()
  } , []);

  return (
    <div className="landing-page__container">
      <Navbar
        className="landing-page__container__navbar"
        listItems={[{ label: "Username", value: "Stefanche" }]}
        id={id}
      />
      <div className="landing-page__container__body">
        <SongList title="Recommended songs" songData={songs}/>
        <SongList title="Liked songs" songData={likedSongs}/>
      </div>
    </div>
  );
};

export default LandingPage;
