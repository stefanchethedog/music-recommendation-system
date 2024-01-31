import { FC } from "react";
import classNames from "classnames";

import axios from "axios";
import { DELETE_ALBUM } from "../../../endpoints";
import DeleteIcon from "@mui/icons-material/Delete";

import "./AlbumCard.styles.scss";
import useSnackbar from "../../../Hooks/useSnackbar";

export type AlbumProps = {
  id: String;
  name: String;
  artistName: string;
  songs: string[];
  genres: string[];
  className?: String;
};

const AlbumCard: FC<AlbumProps> = ({
  className: classes,
  id,
  name,
  artistName,
  songs,
  genres,
}) => {
  const className = classNames("album", classes);

  const { createSnackbar } = useSnackbar({message: 'Album deleted', errorMessage:'Error'})

  const handleDeleteSong = async () => {
    axios
      .delete(DELETE_ALBUM(id))
      .then(() => {
        console.log("ok");
        createSnackbar({error: false})
      })
      .catch((err) => {
        console.log("err");
        createSnackbar({error: true})
      });
  };

  return (
    <div className={className}>
      <DeleteIcon
        className="album__delete"
        onClick={handleDeleteSong}
      ></DeleteIcon>
      <div className="album__name">{name}</div>
      <div className="album__artist">{artistName}</div>
      <div className="album__songs">
        {songs.map((song, index) => (
          <div className="album__songs__song" id={`${song}${index}`}>
            {song}
          </div>
        ))}
      </div>
      <div className="album__genres">
        {genres.map((genre, index) => (
          <div className="album__genres__genre" id={`${genre}${index}`}>
            {genre}
          </div>
        ))}
      </div>
    </div>
  );
};

export default AlbumCard;
