import { FC, useState } from "react";
import { GenresForm } from "../../components";

import "./Genres.styles.scss";
import { GenreProps } from "../../components/Genres/GenreCard/GenreCard.component";
import useSnackbar from "../../Hooks/useSnackbar";
import axios from "axios";
import { GET_ALL_GENRES } from "../../endpoints";
import { GenresList } from "../../components/Genres";

import './Genres.styles.scss';

const GenresPage: FC = () => {
  const [genres, setGenres] = useState<Omit<GenreProps, "className">[]>();

  const { createSnackbar } = useSnackbar({
    message: "Success",
    errorMessage: "Error",
  });

  const handleLoadGenres = async () => {
    await axios
      .get(GET_ALL_GENRES)
      .then((res) => {
        setGenres(res.data);
        createSnackbar({ error: false });
      })
      .catch((err) => {
        createSnackbar({ error: true });
      });
  };
  return (
    <div className="genres-page">
      <GenresForm handleLoadGenres={handleLoadGenres}/>
      {genres && <GenresList title="All genres" genresData={genres} className="genres-page__genres-list"/>}
    </div>
  );
};

export default GenresPage;
