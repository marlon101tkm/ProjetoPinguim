# ProjetoPinguim
 link para documentacao da ML Agents Toolkit -> https://github.com/Unity-Technologies/ml-agents/blob/release_10_docs/docs/Readme.md
 
 links para instalação do pacote ML Agents Toolkit -> https://github.com/Unity-Technologies/ml-agents/blob/release_10_docs/docs/Installation.md
 
 Link para o tutorial original em ingles do Pinguim -> https://www.immersivelimit.com/tutorials/reinforcement-learning-penguins-part-1-unity-ml-agents
 
 Foi  removida a pasta contendo as  bibliotecas para para permitir a importação para GitHub
 
 O projeto contem:<br/>
  As cenas contendo os abientes<br/>
  os objetos Mesh 3D utilizados nas cenas<br/>
  os Codigos fonte do agente e as outras entidades<br/>
  os arquivos contendo os resultdos analizaveis pelo tensorflow <br/>
  os modelos de rede neural  pré treinados <br/>
  
  
  Para executar deve seguir os tutorias de instalação dos ambientes  disponiveis nos links a cima <br/>
   as seguintes ferramentas são necessartias:<br/>
   Unity<br/>
   Anaconda<br/>
   Pytorch<br/>
   Tensorflow<br/>



   Agora foi Adicionado executaveis para cada tipo de rede neural pre treinada e mais um modulo extra sem rede neural , mas com os comandos controlaveis pelo unsuario
   
   As pastas são:<br/>
        Build_Unico: contem a build treinada com uma unica instancia do ambiente<br/>
        Build_Paralelo_8: contem a build treinada com uma 8 copias das instancias do ambiente<br/>
        Build_Paralelo_16: contem a build treinada com uma 16 copias das instancias do ambiente<br/>
        Build_Controlavel: contem a build sem treinamento mas que pode ser controlada pelo usuario<br/>

   para executar utilize o arquivo Pinguim.exe que pode ser encontrado dentro de cada pasta de build<br/>

   para sair da execução de uma build utilize o comando Alt+F4 <br/>

   na build controlavel utilize as teclas WAD para mover o pinguim <br/>
