import { FC, useState } from "react";
import { SongList, SongsForm } from "../../components";
import { GET_SONGS } from "../../endpoints";

import "./Songs.styles.scss";
import { SongProps } from "../../components/Song/SongCard/Song.component";
import axios from "axios";
import useSnackbar from "../../Hooks/useSnackbar";

const SongsPage: FC = () => {
  const [songs, setSongs] = useState<Omit<SongProps, "className">[]>();
  const { createSnackbar } = useSnackbar({
    message: "Successfuly fetched songs",
    errorMessage: "Error, couldn't delete artist",
  });


  const handleLoadSongs = async () => {
    axios.get(GET_SONGS).then((res)=>{
      setSongs(res.data);
      createSnackbar({error: false})
      console.log(res.data);
    }).catch((err)=>{
      console.log(err);
      createSnackbar({error: true})
    })
  };

  return (
    <div className="songs-page">
      <SongsForm handleLoadSongs={handleLoadSongs} />
      {songs && songs.length > 0 && (
        <SongList songData={songs} title="All songs" />
      )}
    </div>
  );
};

export default SongsPage;
