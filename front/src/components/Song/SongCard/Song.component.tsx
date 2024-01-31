import { FC } from "react";
import classNames from "classnames";

import Delete from "@mui/icons-material/Delete";
import useSnackbar from "../../../Hooks/useSnackbar";
import axios from "axios";
import { DELETE_SONG } from "../../../endpoints";

import "./Song.styles.scss";

export type SongProps = {
  id: String;
  name: String;
  genres: String[];
  author: String;
  album?: String;
  className?: String;
};

const Song: FC<SongProps> = ({ className, name, genres, author, album, id }) => {
  const classes = classNames("song__container", className);

  const { createSnackbar } = useSnackbar({
    message: "Song deleted.",
    errorMessage: "Error, couldn't delete song",
  });

  const handleDeleteSong = async () => {
    axios
      .delete(DELETE_SONG(id))
      .then(() => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };

  return (
    <div className={classes}>
      <Delete className="song__container__delete" onClick={handleDeleteSong}/>
      <div className="song__container__title">{name}</div>
      <div className="song__container__author">{author}</div>
      {album && <div className="song__container__author">{album}</div>}
      <div className="song__container__genres">
        {genres.map((genre) => (
          <div className="song__container__genres__genre">{genre} </div>
        ))}
      </div>
    </div>
  );
};

export default Song;
