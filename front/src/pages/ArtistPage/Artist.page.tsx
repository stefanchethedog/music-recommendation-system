import { FC, useState } from "react";
import { ArtistForm } from "../../components";
import { GET_ALL_ARTISTS } from "../../endpoints";

import "./Artist.styles.scss";
import { ArtistProps } from "../../components/Artist/ArtistCard/ArtistCard.component";
import axios from "axios";
import useSnackbar from "../../Hooks/useSnackbar";
import ArtistList from "../../components/Artist/ArtistList";

const ArtistPage: FC = () => {
  const [artists, setArtists] = useState<Omit<ArtistProps, "className">[]>();

  const { createSnackbar } = useSnackbar({
    message: "Success",
    errorMessage: "Error",
  });

  const handleLoadArtists = async () => {
    await axios
      .get(GET_ALL_ARTISTS)
      .then((res) => {
        setArtists(res.data);
        createSnackbar({ error: false });
        console.log(res.data)
        console.log(artists)
      })
      .catch((err) => {
        createSnackbar({ error: true });
      });
  };
  return (
    <div className="artist-page">
      <ArtistForm handleLoadArtists={handleLoadArtists} />
      {artists && <ArtistList title="All artists" artistsData={artists} className="artist-page__artist-list"/>}
    </div>
  );
};

export default ArtistPage;
