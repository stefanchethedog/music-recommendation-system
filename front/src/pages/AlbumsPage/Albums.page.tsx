import { FC, useState } from "react";
import { AlbumsForm } from "../../components";

import "./Albums.styles.scss";
import { AlbumProps } from "../../components/Album/AlbumCard/AlbumCard.component";
import axios from "axios";
import { GET_ALL_ALBUMS } from "../../endpoints";
import useSnackbar from "../../Hooks/useSnackbar";
import { AlbumList } from "../../components/Album";

const AlbumsPage: FC = () => {
  const { createSnackbar } = useSnackbar({
    message: "Success",
    errorMessage: "Not good",
  });

  const [albums, setAlbums] = useState<Omit<AlbumProps, "className">[]>();

  const handleLoadGenres = async () => {
    axios
      .get(GET_ALL_ALBUMS)
      .then(async (res) => {
        setAlbums(res.data);
        createSnackbar({ error: false });
      })
      .catch((err) => {
        console.log(err);
        createSnackbar({ error: true });
      });
  };
  return (
    <div className="albums-page">
      <AlbumsForm handleLoadGenres={handleLoadGenres} />
      {albums && albums.length > 0 && (
        <AlbumList title="All albums" albumsData={albums} />
      )}
    </div>
  );
};

export default AlbumsPage;
